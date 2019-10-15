using System;

namespace Microsoft.Exchange.WebServices.Data
{
    /// <summary>
    /// Represents the MailTips of an individual recipient.
    /// </summary>
    public sealed class MailTipsResponseMessage : ServiceResponse
    {
        private Mailbox recipientAddress;
        private bool? mailboxFull, isInvalid, isModerated, deliveryRestricted;
        private string customMailTip, pendingMailTips;
        private int? totalMemberCount, externalMemberCount, maxMessageSize;

        private OutOfOfficeMessage oof;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailTipsResponseMessage"/> class.
        /// </summary>
        internal MailTipsResponseMessage() : base() { }

        /// <summary>
        /// Reads response elements from XML.
        /// </summary>
        /// <param name="reader">
        ///     The reader.
        /// </param>
        internal override void ReadElementsFromXml(EwsServiceXmlReader reader)
        {
            base.ReadElementsFromXml(reader);

            reader.ReadStartElement(XmlNamespace.Messages, XmlElementNames.MailTips);
            reader.ReadStartElement(XmlNamespace.Types, XmlElementNames.RecipientAddress);
            reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.Name);
            var email = reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.EmailAddress);
            var routing = reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.RoutingType);
            recipientAddress = new Mailbox(email, routing);

            reader.ReadEndElementIfNecessary(XmlNamespace.Types, XmlElementNames.RecipientAddress);
            pendingMailTips = reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.PendingMailTips);
            reader.Read();

            if (reader.IsStartElement(XmlNamespace.Types, XmlElementNames.MailboxFull))
            {
                var mfTextValue = reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.MailboxFull);
                mailboxFull = System.Convert.ToBoolean(mfTextValue);
                reader.Read();
            }
            if (reader.IsStartElement(XmlNamespace.Types, XmlElementNames.CustomMailTip))
            {
                customMailTip = reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.CustomMailTip);
                reader.Read();
            }
            if (reader.IsStartElement(XmlNamespace.Types, XmlElementNames.TotalMemberCount))
            {
                var textValue = reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.TotalMemberCount);
                totalMemberCount = System.Convert.ToInt32(textValue);
                reader.Read();
            }
            if (reader.IsStartElement(XmlNamespace.Types, XmlElementNames.MaxMessageSize))
            {
                var textValue = reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.MaxMessageSize);
                maxMessageSize = System.Convert.ToInt32(textValue);
                reader.Read();
            }
            if (reader.IsStartElement(XmlNamespace.Types, XmlElementNames.DeliveryRestricted))
            {
                var restrictionTextValue = reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.DeliveryRestricted);
                deliveryRestricted = System.Convert.ToBoolean(restrictionTextValue);
                reader.Read();
            }
            if (reader.IsStartElement(XmlNamespace.Types, XmlElementNames.IsModerated))
            {
                var moderationTextValue = reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.IsModerated);
                isModerated = System.Convert.ToBoolean(moderationTextValue);
                reader.Read();
            }
            if (reader.IsStartElement(XmlNamespace.Types, XmlElementNames.InvalidRecipient))
            {
                var invalidRecipientTextValue = reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.InvalidRecipient);
                isInvalid = System.Convert.ToBoolean(invalidRecipientTextValue);
                reader.Read();
            }
            if (reader.IsStartElement(XmlNamespace.Types, XmlElementNames.ExternalMemberCount))
            {
                var textValue = reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.ExternalMemberCount);
                externalMemberCount = System.Convert.ToInt32(textValue);
                reader.Read();
            }
            if (reader.IsStartElement(XmlNamespace.Types, XmlElementNames.OutOfOffice))
            {
                reader.Read();

                if (reader.IsStartElement(XmlNamespace.Types, XmlElementNames.ReplayBody))
                {
                    var msg = reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.Message);
                    DateTime? startTime = null;
                    DateTime? endTime = null;
                    reader.Read();
                    reader.Read();
                    if (reader.IsStartElement(XmlNamespace.Types, XmlElementNames.Duration))
                    {
                        startTime = DateTime.Parse(reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.StartTime));
                        endTime = DateTime.Parse(reader.ReadElementValue(XmlNamespace.Types, XmlElementNames.EndTime));
                        reader.Read();
                        reader.Read();
                    }
                    oof = new OutOfOfficeMessage(msg, startTime, endTime);
                    reader.Read();
                }
            }
            reader.ReadEndElementIfNecessary(XmlNamespace.Messages, XmlElementNames.MailTips);
        }

        // MailTips node: https://msdn.microsoft.com/en-us/library/dd899507(v=exchg.140).aspx

        /// <summary>
        /// Represents the mailbox of the recipient.
        /// </summary>
        public Mailbox RecipientAddress { get { return recipientAddress; } }

        /// <summary>
        /// Indicates that the mail tips in this element could not be evaluated before the server's processing timeout expired.
        /// </summary>
        public string PendingMailTips { get { return pendingMailTips; } }

        /// <summary>
        /// Represents the response message and a duration time for sending the response message.
        /// </summary>
        public OutOfOfficeMessage OutOfOffice { get { return oof; } }

        /// <summary>
        /// Indicates whether the mailbox for the recipient is full.
        /// </summary>
        public bool? MailboxFull { get { return mailboxFull; } }

        /// <summary>
        /// Represents a customized mail tip message.
        /// </summary>
        public string CustomMailTip { get { return customMailTip; } }

        /// <summary>
        /// Represents the count of all members in a group.
        /// </summary>
        public int? TotalMemberCount { get { return totalMemberCount; } }

        /// <summary>
        /// Represents the count of external members in a group.
        /// </summary>
        public int? ExternalMemberCount { get { return externalMemberCount; } }

        /// <summary>
        /// Represents the maximum message size the recipient can accept.
        /// </summary>
        public int? MaxMessageSize { get { return maxMessageSize; } }

        /// <summary>
        /// Indicates whether delivery restrictions will prevent the sender's message from reaching the recipient.
        /// </summary>
        public bool? DeliveryRestricted { get { return deliveryRestricted; } }

        /// <summary>
        /// Indicates whether the recipient's mailbox is being moderated.
        /// </summary>
        public bool? IsModerated { get { return isModerated; } }

        /// <summary>
        /// Indicates whether the recipient is invalid.
        /// </summary>
        public bool? InvalidRecipient { get { return isInvalid; } }
    }

    public class OutOfOfficeMessage
    {
        public OutOfOfficeMessage(string message, DateTime? startTime, DateTime? endTime)
        {
            Message = message;
            StartTime = startTime;
            EndTime = endTime;
        }

        public string Message { get; }

        public DateTime? StartTime { get; }

        public DateTime? EndTime { get; }
    }
}