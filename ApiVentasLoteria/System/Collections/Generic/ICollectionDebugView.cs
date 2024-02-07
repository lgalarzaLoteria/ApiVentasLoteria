using Newtonsoft.Json.Linq;

namespace System.Collections.Generic
{
    internal class ICollectionDebugView<T>
    {
        private IList<JToken> childrenTokens;

        public ICollectionDebugView(IList<JToken> childrenTokens)
        {
            this.childrenTokens = childrenTokens;
        }
    }
}