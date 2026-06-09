using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class FieldSLD : SaveLoadData
{
    public FieldSLD(string id, List<bool> data) : base(id, new object[] { data }) { }
}