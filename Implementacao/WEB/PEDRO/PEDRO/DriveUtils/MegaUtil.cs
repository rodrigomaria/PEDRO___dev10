using CG.Web.MegaApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PEDRO.DriveUtils
{
    public class MegaUtil
    {        

            private String user;
            private String pass;
            MegaApiClient client;
            IEnumerable<INode> nodes;
            INode root;

            public MegaUtil(string user, string pass)
            {
                client = new MegaApiClient();
                client.Login(
                    "nuvem.pedro@gmail.com",
                    "pedroehdemais");
                nodes = client.GetNodes();
                root = nodes.Single(n => n.Type == NodeType.Root);
            }

            public void upload(string path, string nome)
            {
                client.UploadFile(path, root);
            }

            public void DeleteFile(string nome)
            {
                INode file = nodes.Single(n => n.Name == nome);
                client.Delete(file, false);
            }
        
    }
}