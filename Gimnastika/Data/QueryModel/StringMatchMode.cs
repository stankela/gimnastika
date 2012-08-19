using System;

namespace Gimnastika.Data.QueryModel
{
    [Serializable]
    public enum StringMatchMode
    {
        Exact,
        Anywhere,
        End,
        Start
    }
}
