

namespace MVCProject.Api.Controllers.Configuration
{
    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    #endregion
    public class DocumentMasterController : BaseController
    {
        private MVCProjectEntities entities;

        public DocumentMasterController()
        {
            this.entities = new MVCProjectEntities();
        }

        /// <summary>
        /// Get Document Type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse GetDocumentType()
        {
            var result = this.entities.DocumentTypeMaster.Where(x => x.IsActive.Value).ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, result);
        }

        /// <summary>
        /// Add and Update Document Type
        /// </summary>
        /// <param name="DocumentDetail"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse SaveDocumentDetails(DocumentTypeMaster DocumentDetail)
        {
            if (this.entities.DocumentTypeMaster.Any(x => x.DocumentId != DocumentDetail.DocumentId && x.DocumentName.Trim() == DocumentDetail.DocumentName.Trim()))
            {
                return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Document));

            }
            else
            {
                DocumentTypeMaster existingDocumentDetail = this.entities.DocumentTypeMaster.Where(x => x.DocumentId == DocumentDetail.DocumentId).FirstOrDefault();
                if (existingDocumentDetail == null)
                {

                    this.entities.DocumentTypeMaster.AddObject(DocumentDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Document));
                    }
                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Document));
                }

                // For Update
                else
                {
                    existingDocumentDetail.DocumentName = DocumentDetail.DocumentName;
                    this.entities.DocumentTypeMaster.ApplyCurrentValues(existingDocumentDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Document));
                    }
                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Document));
                }
            }

        }

        /// <summary>
        /// Get Document By Document Id
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse GetDocumentById(int documentId)
        {
            var doucment = this.entities.DocumentTypeMaster.Where(x => x.DocumentId == documentId).Select(x => new {
                DocumentId = x.DocumentId,
                DocumentName = x.DocumentName,
                IsActive = x.IsActive
            }).SingleOrDefault();

            if (doucment != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, doucment);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        /// <summary>
        /// Get All Documents
        /// </summary>
        /// <param name="DocumentpagingParams"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse GetAllDocuments(PagingParams DocumentpagingParams)
        {
            if (string.IsNullOrWhiteSpace(DocumentpagingParams.Search))
            {
                DocumentpagingParams.Search = string.Empty;
            }
            var DocuemntMasterlist = (from d in this.entities.DocumentTypeMaster.AsEnumerable().Where(x => x.DocumentName.Trim().ToLower().Contains(DocumentpagingParams.Search.Trim().ToLower()))
                                      let TotalRecords = this.entities.DocumentTypeMaster.AsEnumerable().Where(x => x.DocumentName.Trim().ToLower().Contains(DocumentpagingParams.Search.Trim().ToLower())).Count()

                                      select new
                                      {
                                          DocumentId = d.DocumentId,
                                          DocumentName = d.DocumentName,
                                          IsActive = d.IsActive
                                      }).AsQueryable().OrderByField(DocumentpagingParams.OrderByColumn, DocumentpagingParams.IsAscending).Skip((DocumentpagingParams.CurrentPageNumber = -1) * DocumentpagingParams.PageSize).Take(DocumentpagingParams.PageSize);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, DocuemntMasterlist);

        }

        /// Disposes expensive resources.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.entities != null)
                {
                    this.entities.Dispose();
                }
            }

            base.Dispose(disposing);
        }

    }
}
