using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FMODUnity
{
    [Serializable]
    public class BankRef
    {
        public string Name;

        public BankRef(string _name)
        {
            Name = _name;
        }
    }

    [Serializable]
    public class BankRefList : IEnumerable<BankRef>
    {
        public List<BankRef> Banks;
        public bool AllBanks = true;

        public IEnumerator<BankRef> GetEnumerator()
        {
            if (AllBanks)
            {
                return Settings.Instance.Banks.GetEnumerator();
            }
            else
            {
                return Banks.GetEnumerator();
            }

        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
