
namespace Microsoft.Exchange.WebServices.Data
{
    /// <summary>
    /// Represents the results of a GetMailTips operation.
    /// </summary>
    public sealed class GetMailTipsResults : ServiceResponse
    {
        private ServiceResponseCollection<MailTipsResponseMessage> responseCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMailTipsResults"/> class.
        /// </summary>
        internal GetMailTipsResults():base() { }

        /// <summary>
        /// Reads response elements from XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        internal override void ReadElementsFromXml(EwsServiceXmlReader reader)
        {
            responseCollection = new ServiceResponseCollection<MailTipsResponseMessage>();
            base.ReadElementsFromXml(reader);
            reader.ReadStartElement(XmlNamespace.Messages, XmlElementNames.ResponseMessages);

            if (!reader.IsEmptyElement)
            {
                // Because we don't have count of returned objects
                // test the element to determine if it is return object or EndElement
                reader.Read();
                while (reader.IsStartElement(XmlNamespace.Messages, XmlElementNames.MailTipsResponseMessageType))
                {
                    MailTipsResponseMessage mrm = new MailTipsResponseMessage();
                    mrm.LoadFromXml(reader, XmlElementNames.MailTipsResponseMessageType);
                    this.responseCollection.Add(mrm);
                    reader.Read();
                }
                reader.EnsureCurrentNodeIsEndElement(XmlNamespace.Messages, XmlElementNames.ResponseMessages);
            }
        }

        /// <summary>
        /// Gets a collection of MailTips responses for the requested recipients
        /// </summary>
        public ServiceResponseCollection<MailTipsResponseMessage> MailTipsResponses
        {
            get { return responseCollection; }
            internal set { responseCollection = value; }
        }
    }
}
