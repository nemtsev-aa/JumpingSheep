using System;

public class GeneralIndicators {
    public string UserName;
    public DateTime LastVisitDate;
    public int Gold;

    public GeneralIndicators() {

    }
     
    public GeneralIndicators(string userName, DateTime lastVisitDate, int gold) {
        UserName = userName;
        LastVisitDate = lastVisitDate;
        Gold = gold;
    }
}
