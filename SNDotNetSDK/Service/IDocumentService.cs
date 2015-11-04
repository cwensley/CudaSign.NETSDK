using SNDotNetSDK.Models;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SNDotNetSDK.Service
{
    /**
     * Created by Deepak on 5/14/2015
     * 
     * This interface is used to perform to Document specific operations in the SignNow Application.
     */
    public interface IDocumentService
    {
		Document Create(Oauth2Token token, string fileName, byte[] data);

		Document Create(Oauth2Token token, string fileName, Stream stream);

		Document Create(Oauth2Token token, string fileName);

        IEnumerable<Document> GetDocuments(Oauth2Token token);

        Document GetDocumentbyId(Oauth2Token token, string id);

        string UpdateDocument(Oauth2Token token, DocumentFieldMap fieldsMap, string id);

        string Invite(Oauth2Token token, Invitation invitation, string id, bool sendEmail = false);

        string RoleBasedInvite(Oauth2Token token, EmailSignature emailSignature, string id, bool sendEmail = false);

        string CancelInvite(Oauth2Token token, string id);

        Document ShareDocument(Oauth2Token token, string id);

        IEnumerable<DocumentHistory> GetDocumentHistory(Oauth2Token token, string id);

        Template CreateTemplate(Oauth2Token token, Template template);

        Template CreateNewDocumentFromTemplate(Oauth2Token token, Template template);

        byte[] DownloadCollapsedDocument(Oauth2Token token, string id);

        string DeleteDocument(Oauth2Token token, string id);

        byte[] MergeDocuments(Oauth2Token token, Hashtable myMergeMap);

        EventSubscription CreateEventSubscription(Oauth2Token token, EventSubscription events);

        EventSubscription DeleteEventSubscription(Oauth2Token token, string id);

        Document CreateSimpleFieldTag(Oauth2Token token, Document documentPath);

		Document CreateDocumentFieldExtract(Oauth2Token token, string filePath);

		Document CreateDocumentFieldExtract(Oauth2Token token, string fileName, byte[] data);

		Document CreateDocumentFieldExtract(Oauth2Token token, string fileName, Stream stream);
	}
}