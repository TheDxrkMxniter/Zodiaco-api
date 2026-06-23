namespace Zodiaco.Api.Common;

public static class TruckStatusValues
{
    public const string Available = "AVAILABLE";
    public const string Reserved = "RESERVED";
    public const string Sold = "SOLD";
    public const string Hidden = "HIDDEN";
    public const string UnderReview = "UNDER_REVIEW";
}

public static class LeadStatusValues
{
    public const string PendingReview = "PENDING_REVIEW";
    public const string FollowUp = "FOLLOW_UP";
    public const string NoResponse = "NO_RESPONSE";
}

public static class LeadTypeValues
{
    public const string General = "GENERAL";
    public const string InventoryInterest = "INVENTORY_INTEREST";
    public const string Contact = "CONTACT";
}

public static class PaymentOptionValues
{
    public const string Cash = "CASH";
    public const string Financing = "FINANCING";
    public const string CashAndFinancing = "CASH_AND_FINANCING";
}

public static class TruckCurrencyValues
{
    public const string Mxn = "MXN";
}

public static class TruckCategoryValues
{
    public const string Tractocamion = "Tractocamion";
    public const string CamionRabon = "Camion rabon";
    public const string CamionTorton = "Camion torton";
    public const string AutobusUrbano = "Autobus urbano";
    public const string AutobusSuburbano = "Autobus suburbano";
    public const string AutobusForaneo = "Autobus foraneo";
    public const string RemolqueCajaSeca = "Remolque caja seca";
    public const string RemolquePlataforma = "Remolque plataforma";
    public const string RemolqueVolteo = "Remolque volteo";
    public const string Dolly = "Dolly";
    public const string MaquinariaConstruccion = "Maquinaria de construccion";
    public const string Otro = "Otro";
}
