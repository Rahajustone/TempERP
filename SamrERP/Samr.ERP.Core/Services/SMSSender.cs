using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JulMar.Smpp;
using JulMar.Smpp.Elements;
using JulMar.Smpp.Esme;
using JulMar.Smpp.Pdu;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class SMSSender:ISMSSender
    {
        private readonly EsmeSession _smppSession;
        private readonly ISMPPSettingService _smppSettingService;
        private SMPPSetting _defaultSMMPSetting;

        private SMPPSetting DefaultSMPPSetting => _defaultSMMPSetting ?? (_defaultSMMPSetting = _smppSettingService.GetDefaultSMPPSetting());

        public SMSSender(
            EsmeSession  smppSession,
            ISMPPSettingService smppSettingService,
            IUnitOfWork unitOfWork
            )
        {
            //
            _smppSession = smppSession;
            _smppSettingService = smppSettingService;
        }




        //public bool ConnectToSMMProvider(string address, int port, string systemId, string password)
        //{
        //    _smppSession.SmppVersion = SmppVersion.SMPP_V34;

        //    _smppSession.Connect(address, port);

        //    if (_smppSession.IsConnected)
        //    {
        //        BindTranceiver(systemId, password);

        //        return true;
        //    }

        //    return _smppSession.IsConnected;
        //}

        //private void BindTranceiver(string systemId, string password)
        //{
        //    bind_transceiver bindPdu = new bind_transceiver(systemId, password, "",
        //        new interface_version(),
        //        new address_range());

        //    if (bindPdu.IsValid)
        //    {
        //        _smppSession.BindTransceiver(bindPdu);
        //    }
        //}
        private async Task AddEmailMessageHistory(User destUser, string subject, string message)
        {

            var emailMessageHistory = new EmailMessageHistory()
            {
                ReceiverUserId = destUser.Id,
                EmailSettingId = _defaultSMMPSetting.Id,
                Subject = subject,
                Message = message,
                ReceiverEmail = destUser.Email
            };
            if (_userProvider.CurrentUser == null)
            {
                var firstUser = await _unitOfWork.Users.All().FirstAsync();
                emailMessageHistory.CreatedUserId = firstUser.Id;
                _unitOfWork.EmailMessageHistories.Add(emailMessageHistory, false);
            }
            else
            {
                _unitOfWork.EmailMessageHistories.Add(emailMessageHistory);

            }

            _unitOfWork.EmailMessageHistories.Add(emailMessageHistory, false);

            await _unitOfWork.CommitAsync();
        }

        private void SendMessageAsync(string phoneNumber, string message)
        {
            submit_sm submitPdu = new submit_sm();

            submitPdu.SourceAddress = new address(TypeOfNumber.ALPHANUMERIC, NumericPlanIndicator.E164, "SMCS.TJ");
            submitPdu.DestinationAddress = new address(TypeOfNumber.INTERNATIONAL, NumericPlanIndicator.E164, phoneNumber);
            submitPdu.RegisteredDelivery = new registered_delivery(DeliveryReceiptType.FINAL_DELIVERY_RECEIPT,
                AcknowledgementType.DELIVERY_USER_ACK_REQUEST, true);
            //mBloxOperatorId operatorId = new mBloxOperatorId("SMCS.TJ");
            //submitPdu.AddVendorSpecificElements(operatorId);
            submitPdu.DataCoding = DataEncoding.LATIN;

            submitPdu.Message = message;

            _smppSession.SmppVersion = SmppVersion.SMPP_V34;

            _smppSession.Connect(_defaultSMMPSetting.Host, _defaultSMMPSetting.PortNumber);

            if (_smppSession.IsConnected)
            {
                bind_transceiver bindPdu = new bind_transceiver(_defaultSMMPSetting.SystemId,
                    _defaultSMMPSetting.Password, "",
                    new interface_version(),
                    new address_range());
                if (bindPdu.IsValid)
                {
                    _smppSession.BindTransceiver(bindPdu);
                    _smppSession.SubmitSm(submitPdu);
                }
            }
        }
    }

    public interface ISMSSender
    {
    }
}
