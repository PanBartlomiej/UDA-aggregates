using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.UserDefined,
   IsInvariantToNulls = true,
   IsInvariantToDuplicates = false,
   IsInvariantToOrder = false,
   MaxByteSize = 8000)]
public struct Wszystkie : IBinarySerialize
{
    private List<double> tab;

    public void Init()
    {
        this.tab = new List<double>();

    }

    public void Accumulate(SqlDouble Value)
    {
        tab.Add((double)Value);

    }

    public void Merge(Wszystkie Group)
    {
        for (int i = 0; i < Group.tab.Count; i++)
            this.tab.Add((double)tab[i]);

    }

    public SqlString Terminate()
    {
        SqlString str = "";
        for (int i = 0; i < tab.Count; i++)
        {
            str += " "+tab[i].ToString() + ";";
        }
        return str;

    }
    public void Read(BinaryReader r)
    {
        int size = (int)r.ReadInt32();
        tab = new List<double>(); 

        for (int i = 0; i < size; i++)
            this.tab.Add(r.ReadDouble());

    }
    public void Write(BinaryWriter w)
    {
        w.Write((int)this.tab.Count);

        for (int i = 0; i < this.tab.Count; i++)
        {
            w.Write((double)this.tab[i]);
        }
    }

}
