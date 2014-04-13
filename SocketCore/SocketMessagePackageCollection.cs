using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocketCore
{
    class SocketMessagePackageCollection : IEnumerable<SocketMessagePackage>
    {
        private List<SocketMessagePackage> _pageList = new List<SocketMessagePackage>();

        public SocketMessagePackageCollection()
        {

        }

        public void AddPackage(SocketMessagePackage messagePackage)
        {
            _pageList.Add(messagePackage);
        }

        public IEnumerable<byte> GetMessageBytes()
        {
            foreach (var packageItem in this)
            {
                foreach (var byteItem in packageItem.Buffer)
                {
                    yield return byteItem;
                }
            }
        }

        public IEnumerator<SocketMessagePackage> GetEnumerator()
        {
            return _pageList.OrderBy(item => item.Timestamp).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}