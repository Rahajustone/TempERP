using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JulMar.Smpp;
using JulMar.Smpp.Elements;
using JulMar.Smpp.Esme;
using JulMar.Smpp.Pdu;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Extensions;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Core.Services
{
    public class SMSSender:ISMSSender
    {
        private readonly EsmeSession _smppSession;
        private readonly ISMPPSettingService _smppSettingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserProvider _userProvider;
        private SMPPSetting _defaultSMMPSetting;

        private SMPPSetting DefaultSMPPSetting => _defaultSMMPSetting ?? (_defaultSMMPSetting = _smppSettingService.GetDefaultSMPPSetting());

        public SMSSender(
            EsmeSession  smppSession,
            ISMPPSettingService smppSettingService,
            IUnitOfWork unitOfWork,
            UserProvider userProvider
            )
        {
            //
            _smppSession = smppSession;
            _smppSettingService = smppSettingService;
            _unitOfWork = unitOfWork;
            _userProvider = userProvider;
        }

        public async Task SendSMSToUserAsync(User destUser, string message, bool hideMessage = false)
        {
            await AddSMSMessageHistory(destUser, message, hideMessage);
            SendMessage(destUser.PhoneNumber,message);

        }
        private async Task AddSMSMessageHistory(User destUser, string message,bool hideMessage = false)
        {
            var emailMessageHistory = new SMSMessageHistory()
            {
                ReceiverUserId = destUser.Id,
                SMPPSettingId = DefaultSMPPSetting.Id,
                Message = hideMessage ? "*****" : message,
                ReceiverPhoneNumber = destUser.Email,
            };
            if (_userProvider.CurrentUser == null)
            {
                var systemUser = await _unitOfWork.Users.GetByIdAsync(GuidExtensions.FULL_GUID);
                emailMessageHistory.CreatedUserId = systemUser.Id;
                _unitOfWork.SMSMessageHistories.Add(emailMessageHistory, false);
            }
            else
            {
                _unitOfWork.SMSMessageHistories.Add(emailMessageHistory);
            }

            await _unitOfWork.CommitAsync();
        }

        private void SendMessage(string phoneNumber, string message)
        {
            submit_sm submitPdu = new submit_sm
            {
                SourceAddress = new address(TypeOfNumber.ALPHANUMERIC, NumericPlanIndicator.E164, "SMCS.TJ"),
                DestinationAddress = new address(TypeOfNumber.INTERNATIONAL, NumericPlanIndicator.E164, phoneNumber),
                RegisteredDelivery = new registered_delivery(DeliveryReceiptType.FINAL_DELIVERY_RECEIPT,
                    AcknowledgementType.DELIVERY_USER_ACK_REQUEST, true),
                DataCoding = DataEncoding.LATIN,
                Message = message
            };

            //mBloxOperatorId operatorId = new mBloxOperatorId("SMCS.TJ");
            //submitPdu.AddVendorSpecificElements(operatorId);


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
}
