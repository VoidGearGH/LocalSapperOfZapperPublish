using System.Collections.Generic;
using UnityEngine;

public class OpenedFieldSLD : SaveLoadData
{
    public OpenedFieldSLD(string id, List<bool> data) : base(id, new object[] { data }) { }
}
