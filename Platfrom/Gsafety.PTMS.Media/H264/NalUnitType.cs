namespace Gsafety.PTMS.Media.H264
{
    // See ITU-T H.264 (04/2013) Table 7-1
    public enum NalUnitType
    {
        Uns = 0,
        Slice = 1,
        Dpa = 2,
        Dpb = 3,
        Dpc = 4,
        Idr = 5,
        Sei = 6,
        Sps = 7,
        Pps = 8,
        Aud = 9,
        EoSeq = 10,
        EoStream = 11,
        Fill = 12,
        SpsExt = 13,
        Prefix = 14,
        SubSps = 15,
        Rsv16 = 16,
        Rsv17 = 17,
        Rsv18 = 18,
        SlcAux = 19,
        SlcExt = 20,
        SlcDv = 21,
        Rsv22 = 22,
        Rsv23 = 23,
        Vdrd = 24
    }
}
