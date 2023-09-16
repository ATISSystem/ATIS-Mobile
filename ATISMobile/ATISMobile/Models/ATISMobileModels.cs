


using System;
using System.Collections.Generic;
using System.Text;



namespace ATISMobile.Models
{
    public class LoadCapacitorLoad
    {
        public string LoadnEstelamId { get; set; }
        public string LoadCapacitorLoadTitleTargetCityTotalAmount { get; set; }
        public string TransportCompanyTarrifPrice { get; set; }
        public string Description { get; set; }
    }

    public class MessageStruct
    {
        public Boolean ErrorCode { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public string Message3 { get; set; }
    }

    public class LoadAllocationsforTruckDriver
    {
        public string LoadAllocationId { get; set; }
        public string Description { get; set; }
        public System.Drawing.Color DescriptionColor { get; set; }
    }

    public class TruckDriver
    {
        public string NameFamily { get; set; }
        public string FatherName { get; set; }
        public string SmartCardNo { get; set; }
        public string NationalCode { get; set; }
        public string Tel { get; set; }
        public string DrivingLicenceNo { get; set; }
        public string Address { get; set; }
        public string DriverId { get; set; }

    }

    public class Truck
    {
        public string TruckId { get; set; }
        public string LPString { get; set; }
        public string LoaderTitle { get; set; }
        public string SmartCardNo { get; set; }
        public string AnnouncementHallSubGroups { get; set; }

    }

    public class Turns
    {
        public string TurnId { get; set; }
        public string OtaghdarTurnNumber { get; set; }
        public string TurnDateTime { get; set; }
        public string TurnStatusTitle { get; set; }
        public string LPPString { get; set; }
        public string TruckDriver { get; set; }
    }

    public class Province
    {
        public string ProvinceId { get; set; }
        public string ProvinceTitle { get; set; }
    }

    public class MoneyWalletAccounting
    {
        public string AccountName { get; set; }
        public string AccountDateTime { get; set; }
        public string CurrentCharge { get; set; }
        public string Mblgh { get; set; }
        public string ReminderCharge { get; set; }
        public string ComputerName { get; set; }
        public string UserName { get; set; }
        public string BackGroundColorName { get; set; }
        public string ForeGroundColorName { get; set; }
    }

    public class MobileProcess
    {
        public string PId { get; set; }
        public string PName { get; set; }
        public string PTitle { get; set; }
        public string TargetMobilePage { get; set; }
        public string Description { get; set; }
        public System.Drawing.Color PForeColor { get; set; }
        public System.Drawing.Color PBackColor { get; set; }
    }

    public class PermissionsIssued
    { public string ReportItemHeader { get; set; } public string ReportItemDetails { get; set; } }

    public class DriverSelfDeclaration
    {
        public Int64 DSDId { get; set; }
        public string DSDName { get; set; }
        public string DSDTitle { get; set; }
        public string DefaultValue { get; set; }
        public string DSDValue { get; set; }
        public bool PersianKeyboard { get; set; }
        public bool IsNumeric { get; set; }
        public bool DecimalPoint { get; set; }
        public bool HasAttachement { get; set; }
    }

    public class SMSOwnerCurrentState
    {
        public Boolean IsSendingActive { get; set; }
        public string TextToView { get; set; }
        public string TextToViewColor { get; set; }
        public string SMSOwnerChargeMessage { get; set; }

    }


}
