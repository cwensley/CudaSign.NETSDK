using Microsoft.VisualStudio.TestTools.UnitTesting;
using SNDotNetSDK.Configuration;
using SNDotNetSDK.Models;
using SNDotNetSDK.Service;
using SNDotNetSDK.ServiceImpl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace SNDotNetSDK.Test
{
    /**
     * Created by Deepak on 5/14/2015
     * 
     * This test class is used to perform and test the Document specific test operations.
     */
    [TestClass]
    public class DocumentServiceTest
    {
        static CudaSign cudasign;
        private string InputdirPath = "InputdirPath";
        private string OutputdirPath = "OutputdirPath";

        [ClassInitialize]
        public static void Before(TestContext t)
        {
            Config config = new Config("apiBase", "clientId", "clientSecret");
            cudasign = new CudaSign(config);
        }
        
        /*
        * This test method is used to create a Document in SignNow Application
        */
       [TestMethod]
        public void CreateDocument()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if(Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);
        }

        /*
         * This test method is used to get all the documents in the form of array for the specified user.
         */
        [TestMethod]
        public void GetDocuments()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);

            Document[] resultDoc = cudasign.DocumentService.GetDocuments(requestedToken).ToArray();
            Assert.IsNotNull("resultDocid's", resultDoc.Length.ToString());
        }

        /*
         * This test method is used to GET the Document for a given user based on the given DocumentID from SignNow Application
         */
        [TestMethod]
        public void GetDocumentbyId()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);

            Document resultDoc = cudasign.DocumentService.GetDocumentbyId(requestedToken, document.Id);
            Assert.IsNotNull("resultDocid", resultDoc.Id);
        }

        /*
         * This test method is used to update an existing Document in SignNow Application
         */
        [TestMethod]
        public void UpdateDocument()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);

            // Build the data for Texts Test
            Text text = new Text();
            text.Size = 30;
            text.X = 61;
            text.Y = 72;
            text.PageNumber = 0;
            text.Font = "Arial";
            text.Data = "A SAMPLE TEXT FIELD";
            text.LineHeight = 9.075;

            Text text1 = new Text();
            text1.Size = 30;
            text1.X = 61;
            text1.Y = 72;
            text1.PageNumber = 1;
            text1.Font = "Arial";
            text1.Data = "A SAMPLE TEXT FIELD 2";
            text1.LineHeight = 9.075;


            // Build the data for Checks
            Checkbox checks = new Checkbox();
            checks.Width = 20;
            checks.Height = 20;
            checks.X = 234;
            checks.Y = 500;
            checks.PageNumber = 0;

            Checkbox checks1 = new Checkbox();
            checks1.Width = 20;
            checks1.Height = 20;
            checks1.X = 200;
            checks1.Y = 53;
            checks.PageNumber = 1;


            // Creating the Fields

            Radio radiobutton = new Radio();
            radiobutton.PageNumber = 1;
            radiobutton.X = 150;
            radiobutton.Y = 65;
            radiobutton.Width = 40;
            radiobutton.Height = 40;
            radiobutton.Check = 0;
            radiobutton.Value = "apple";
            radiobutton.Created = "123456789";

            Radio radiobutton1 = new Radio();
            radiobutton1.PageNumber = 1;
            radiobutton1.X = 250;
            radiobutton1.Y = 55;
            radiobutton1.Width = 40;
            radiobutton1.Height = 40;
            radiobutton1.Check = 0;
            radiobutton1.Value = "cherry";
            radiobutton1.Created = "123456789";

            Field fields = new Field();
            fields.X = 13;
            fields.Y = 133;
            fields.Width = 25;
            fields.Height = 121;
            fields.PageNumber = 1;
            fields.Role = "buyer";
            fields.Required = true;
            fields.Type = "radiobutton";
			fields.Radio.Add(radiobutton);
			fields.Radio.Add(radiobutton1);

			// create the field map
			var fieldsMap = new DocumentFieldMap
			{
				Fields = { fields },
				Checks = { checks, checks1 },
				Texts = { text, text1 }
			};

            string resultDocId = cudasign.DocumentService.UpdateDocument(requestedToken, fieldsMap, document.Id);

            Assert.IsNotNull("DocumentId", resultDocId);
        }
       /**
        *
        * This utility method is used to convert the image based on its type to a base 64 encoded String.
        */
        protected static string ImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        /*
         * This test method is used to invite the signers to sign the Document in SignNow Application
         */
        [TestMethod]
        public void Invite()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);

            string toEmail = "deepak" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            Invitation invitation = new Invitation();
            invitation.From = resultUser.Email;
            invitation.To = toEmail;

            string resinvite = cudasign.DocumentService.Invite(requestedToken, invitation, document.Id);
            Assert.AreEqual("success", resinvite);
        }

        /*
         * This test method is used to send rolebased invites to the signers to sign on  the document
         */
        [TestMethod]
        public void RoleBasedInvite()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);

            // Build the data for Texts Test
            Text text = new Text();
            text.Size = 30;
            text.X = 61;
            text.Y = 72;
            text.PageNumber = 0;
            text.Font = "Arial";
            text.Data = "A SAMPLE TEXT FIELD";
            text.LineHeight = 9.075;

            Text text1 = new Text();
            text1.Size = 30;
            text1.X = 61;
            text1.Y = 72;
            text1.PageNumber = 1;
            text1.Font = "Arial";
            text1.Data = "A SAMPLE TEXT FIELD 2";
            text1.LineHeight = 9.075;

            // Build the data for Checks
            Checkbox checks = new Checkbox();
            checks.Width = 20;
            checks.Height = 20;
            checks.X = 234;
            checks.Y = 500;
            checks.PageNumber = 0;

            Checkbox checks1 = new Checkbox();
            checks1.Width = 20;
            checks1.Height = 20;
            checks1.X = 200;
            checks1.Y = 53;
            checks.PageNumber = 1;

            // Creating the Fields

            Radio radiobutton = new Radio();
            radiobutton.PageNumber = 1;
            radiobutton.X = 150;
            radiobutton.Y = 65;
            radiobutton.Width = 40;
            radiobutton.Height = 40;
            radiobutton.Check = 0;
            radiobutton.Value = "apple";
            radiobutton.Created = "123456789";

            Radio radiobutton1 = new Radio();
            radiobutton1.PageNumber = 1;
            radiobutton1.X = 250;
            radiobutton1.Y = 55;
            radiobutton1.Width = 40;
            radiobutton1.Height = 40;
            radiobutton1.Check = 0;
            radiobutton1.Value = "cherry";
            radiobutton1.Created = "123456789";

            Field fields = new Field();
            fields.X = 13;
            fields.Y = 133;
            fields.Width = 25;
            fields.Height = 121;
            fields.PageNumber = 1;
            fields.Role = "signer";
            fields.Required = true;
            fields.Type = "radiobutton";
            fields.Radio.Add(radiobutton);
			fields.Radio.Add(radiobutton1);

            Field fields1 = new Field();
            fields1.X = 20;
            fields1.Y = 133;
            fields1.Width = 122;
            fields1.Height = 60;
            fields1.PageNumber = 0;
            fields1.Role = "buyer";
            fields1.Required = true;
            fields1.Type = "initials";

            Field fields2 = new Field();
            fields2.X = 35;
            fields2.Y = 133;
            fields2.Width = 122;
            fields2.Height = 60;
            fields2.PageNumber = 1;
            fields2.Role = "TestingRole";
            fields2.Required = true;
            fields2.Type = "text";

			var fieldsMap = new DocumentFieldMap
			{
				Fields = { fields, fields1, fields2 },
				Checks = { checks, checks1 },
				Texts = { text, text1 }
			};

            var resultDocId = cudasign.DocumentService.UpdateDocument(requestedToken, fieldsMap, document.Id);
            Document getDoc = cudasign.DocumentService.GetDocumentbyId(requestedToken, resultDocId);

            Field[] flds = getDoc.Fields;
            EmailSignature emailSignature = new EmailSignature();
            int counter = 0;
            //iterate over fields
            for(int i=0;i<flds.Length;i++)
            {
                string toEmail = "deepak" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
                    var roleMap = new EmailRole();
                    roleMap.Email = toEmail;
                    roleMap.RoleId = flds[i].RoleId;
                    roleMap.Role = flds[i].Role;
                    roleMap.Order = ++counter;
				emailSignature.To.Add(roleMap);
            }
            emailSignature.From = resultUser.Email;
            string[] ccuser = new string[] { "ccuser1@mailinator.com", "ccuser2@mailinator.com" };
            emailSignature.CC = ccuser;
            emailSignature.Message = resultUser.Email + " asked you to sign this document";
            emailSignature.Subject = "SignNow Invitation";

            string resinvite = cudasign.DocumentService.RoleBasedInvite(requestedToken, emailSignature, document.Id);
            Assert.AreEqual("success", resinvite);
        }

        /*
         * This test method is used to Cancel an invite to a document.
         */
        [TestMethod]
        public void CancelInvite()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);

            string toEmail = "deepak" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            Invitation invitation = new Invitation();
            invitation.From = resultUser.Email;
            invitation.To = toEmail;

            string resinvite = cudasign.DocumentService.Invite(requestedToken, invitation, document.Id);
            Assert.AreEqual("success", resinvite);

            string cancelinvite = cudasign.DocumentService.CancelInvite(requestedToken, document.Id);
            Assert.AreEqual("success", cancelinvite);
        }

        /*
         * This test method is used to GET a Document as PDF in SignNow Application
         */
        [TestMethod]
        public void ShareDocument()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);
            Document resdoc = cudasign.DocumentService.ShareDocument(requestedToken, document.Id);

            Assert.IsNotNull("Document Link", resdoc.Link);
        }

        /*
         *This test method is used to GET the Document History for a given Document ID in SignNow Application
         */
        [TestMethod]
        public void GetDocumentHistory()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);

            DocumentHistory[] dochistory = cudasign.DocumentService.GetDocumentHistory(requestedToken, document.Id).ToArray();
            Assert.IsNotNull("Ip Address :", dochistory[0].IpAddress);
        }

        /*
         *This test method is used to create a Template for a Given Document in SignNow Application
         */
        [TestMethod]
        public void CreateTemplate()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);

            Template template = new Template();
            template.DocumentId = document.Id;
            template.DocumentName = "New Template";

            Template resultTemplate = cudasign.DocumentService.CreateTemplate(requestedToken, template);
            Assert.IsNotNull("template create result", resultTemplate.Id);
        }

        /*
         This test method is used to create a Document from a Template based on the template id SignNow Application
         */
        [TestMethod]
        public void CreateNewDocumentFromTemplate()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);

            Template template = new Template();
            template.DocumentId = document.Id;
            template.DocumentName = "New Template-PostDoc28";

            Template resultTemplate = cudasign.DocumentService.CreateTemplate(requestedToken, template);
            Assert.IsNotNull("template create result", resultTemplate.Id);
            resultTemplate.DocumentName = "Copy Template-PostDoc28";

            Template copyTemplate = cudasign.DocumentService.CreateNewDocumentFromTemplate(requestedToken, resultTemplate);
            Assert.IsNotNull("Document Id", copyTemplate.Id);
        }

        /*
         This test method is used to get the document in the form of bytes which is later transformed to .pdf
         */
        [TestMethod]
        public void DownloadCollapsedDocument()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);

            byte[] docarr = cudasign.DocumentService.DownloadCollapsedDocument(requestedToken, document.Id);
            if(Directory.Exists(OutputdirPath))
            {
                string dest = OutputdirPath + @"\" + document.Id + ".pdf";
                File.WriteAllBytes(dest, docarr);
            }
            Assert.IsNotNull("Document Content", docarr.Length.ToString());
        }

        /*
         This test method is used to delete a previously uploaded document
         */
        [TestMethod]
        public void DeleteDocument()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);

            string confirm = cudasign.DocumentService.DeleteDocument(requestedToken, document.Id);
            Assert.AreEqual("success", confirm);
        }

        /*
         This test method is used to merge the list of documents based on the given document id's in SignNow Application
         */
        [TestMethod]
        public void MergeDocuments()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc1 = new Document();
            Document doc2 = new Document();
            if (Directory.Exists(InputdirPath) && Directory.GetFiles(@InputdirPath).Length>=2)
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc1.FilePath = DocFilePath[0];
                doc2.FilePath = DocFilePath[1];
            }
            Document document1 = cudasign.DocumentService.Create(requestedToken, doc1.FilePath);
            Assert.IsNotNull("DocumentId", document1.Id);
            Document document2 = cudasign.DocumentService.Create(requestedToken, doc2.FilePath);
            Assert.IsNotNull("DocumentId", document2.Id);

            List<string> docIds = new List<string>();
            docIds.Add(document1.Id);
            docIds.Add(document2.Id);
            Hashtable myMergeMap = new Hashtable();
            myMergeMap.Add("document_ids", docIds);

            byte[] res = cudasign.DocumentService.MergeDocuments(requestedToken, myMergeMap);
            if (Directory.Exists(OutputdirPath))
            {
                string dest = OutputdirPath + @"\Merge" + (document1.Id.Substring(1, 4) + document2.Id.Substring(1, 4)) + ".pdf";
                File.WriteAllBytes(dest, res);
            }
            Assert.IsNotNull("Document Content", res.Length.ToString());
        }

        /*
         This test method is used for creating event subscription, which will be triggered when specific event take place
         */
        [TestMethod]
        public void CreateEventSubscription()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            EventSubscription evs = new EventSubscription();
            evs.Event = "document.create";
            evs.CallbackUrl = "https://www.myapp.com/path/to/callback.php";
            EventSubscription res = cudasign.DocumentService.CreateEventSubscription(requestedToken, evs);
            Assert.IsNotNull("Subscription Id Created", res.Id);
        }

        /*
         This test method is used to delete an event subscription 
         */
        [TestMethod]
        public void DeleteEventSubscription()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            EventSubscription evs = new EventSubscription();
            evs.Event = "document.create";
            evs.CallbackUrl = "https://www.myapp.com/path/to/callback.php";
            EventSubscription res = cudasign.DocumentService.CreateEventSubscription(requestedToken, evs);
            Assert.IsNotNull("Subscription Id Created", res.Id);

            EventSubscription deleteEvent = cudasign.DocumentService.DeleteEventSubscription(requestedToken, res.Id);
            Assert.AreEqual("deleted", deleteEvent.Status);
        }

        /*
         This test method is used to upload a file that contains SignNow Document Field Tags and the tags here are only
         * Simple field tags
         */
        [TestMethod]
        public void CreateSimpleFieldTag()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.Create(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);
        }

        /*
        * This test method is used to create a Document in SignNow Application that contains SignNow Document Field Tags.
        */
        [TestMethod]
        public void CreateDocumentFieldExtract()
        {
            string randomEmail = "lukeskywalker" + DateTime.Now.ToBinary().ToString() + "@mailinator.com";
            User user = new User();
            user.Email = randomEmail;
            user.Password = "fakePassword";
            user.FirstName = "firstName";
            user.LastName = "LastName";

            User resultUser = cudasign.UserService.Create(user);

            Assert.IsNotNull("No user id from creating user", resultUser.Id);
            resultUser.Password = "fakePassword";

            Oauth2Token requestedToken = cudasign.AuthenticationService.RequestToken(resultUser);
            Assert.IsNotNull("Access Token", requestedToken.AccessToken);

            Document doc = new Document();
            if (Directory.Exists(InputdirPath))
            {
                string[] DocFilePath = Directory.GetFiles(@InputdirPath);
                doc.FilePath = DocFilePath[0];
            }

            Document document = cudasign.DocumentService.CreateDocumentFieldExtract(requestedToken, doc.FilePath);
            Assert.IsNotNull("DocumentId", document.Id);
        }
    }
}