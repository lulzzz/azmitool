using System;
using System.IO;

using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Identity;
using System.Collections.Generic;

namespace azmi_main
{
    class AzmiException : Exception
    {
        // TODO: Move this to separate folder and file
        // TODO: Create separate classes for each type of exceptions

        public AzmiException(string identity, Exception ex) :
            base(ModifyMessage(identity, ex).message, ModifyMessage(identity, ex).ex)
        {
        }

        private static (string message, Exception ex) ModifyMessage(string identity, Exception ex)
        {
            if (string.IsNullOrEmpty(identity))
            {
                return ("Missing identity argument", ex);
            } else if (ex.Message.Contains("See inner exception for details.")
                  && (ex.InnerException != null)
                  && (ex.InnerException.Message.Contains("Identity not found")))
            {
                return ("Managed identity not found", ex.InnerException.InnerException);
            } else
            {
                return ("General error", ex);
            }
        }
    }


    
    public static class Operations
    {
        // Class defining main operations performed by azmi tool

        public static string getToken(string endpoint = "management", string identity = null)
        {
            var Cred = new ManagedIdentityCredential(identity);
            if (string.IsNullOrEmpty(endpoint)) { endpoint = "management"; };
            var Scope = new String[] { $"https://{endpoint}.azure.com" };
            var Request = new TokenRequestContext(Scope);

            try
            {
                var Token = Cred.GetToken(Request);
                return Token.Token;
            } catch (Exception ex)
            {
                //throw IdentityError(identity, ex);
                throw new AzmiException(identity, ex);
            }
        }

        public static string getBlob(string blobURL, string filePath, string identity = null)
        {
            // Download the blob to a local file
            var Cred = new ManagedIdentityCredential(identity);
            var blobClient = new BlobClient(new Uri(blobURL), Cred);
            try
            {
                blobClient.DownloadTo(filePath);
                return "Success";
            } catch (Exception ex)
            {
                throw new AzmiException(identity, ex);
            }
        }

        public static string setBlob_byContainer(string filePath, string containerUri, string identity = null)
        {
            // sets blob content based on local file content in provided container
            if (!(File.Exists(filePath))) {
                throw new FileNotFoundException($"File '{filePath}' not found!");
            }

            var Cred = new ManagedIdentityCredential(identity);
            var containerClient = new BlobContainerClient(new Uri(containerUri), Cred);
            containerClient.CreateIfNotExists();
            var blobClient = containerClient.GetBlobClient(filePath.TrimStart('/'));
            try
            {
                blobClient.Upload(filePath);
                return "Success";
            } catch (Exception ex)
            {
                throw new AzmiException(identity, ex);
            }
        }

        public static string listBlobs(string containerUri, string identity = null, string prefix = null)
        {            
            var Cred = new ManagedIdentityCredential(identity);
            var containerClient = new BlobContainerClient(new Uri(containerUri), Cred);
            containerClient.CreateIfNotExists();
            var blobNamesList = new List<string>();

            try
            {
                foreach (var blob in containerClient.GetBlobs(prefix: prefix)) {
                    blobNamesList.Add(blob.Name);
                }

                return blobNamesList.Count == 0 ? null : String.Join("\n", blobNamesList);
            } catch (Exception ex)
            {
                throw new AzmiException(identity, ex);
            }
        }

        public static string setBlob_byBlob(string filePath, string blobUri, string identity = null)
        {
            // sets blob content based on local file content with provided blob url
            if (!(File.Exists(filePath)))
            {
                throw new FileNotFoundException($"File '{filePath}' not found!");
            }

            var Cred = new ManagedIdentityCredential(identity);
            var blobClient = new BlobClient(new Uri(blobUri), Cred);
            try
            {
                blobClient.Upload(filePath);
                return "Success";
            } catch (Exception ex)
            {
                throw new AzmiException(identity, ex);
            }
        }
    }
}
