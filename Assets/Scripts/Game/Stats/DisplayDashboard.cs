using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayDashboard : MonoBehaviour
{
    public TextMeshProUGUI TimeText;
    
    public TextMeshProUGUI overallEcologyRateTextText;
    public TextMeshProUGUI overallSocietyRateText;
    
    public TextMeshProUGUI currentMoneyInBankText;
    public TextMeshProUGUI currentEnergyInStockText;
    public TextMeshProUGUI currentEmittedCo2Text;
    public TextMeshProUGUI currentWasteProducedText;
    public TextMeshProUGUI currentGreenSpacesText;
    
    public TextMeshProUGUI maxPopSizeText;
    public TextMeshProUGUI totalCitizensText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI happinessText;
    public TextMeshProUGUI sensibilisationText;
    public TextMeshProUGUI acceptationText;
    public TextMeshProUGUI dailyTransportsUsersText;
    
    public TextMeshProUGUI profitsPerMonthText;
    public TextMeshProUGUI lossesPerMonthText;
    public TextMeshProUGUI energyProductionPerMonthText;
    public TextMeshProUGUI energyConsumptionPerMonthText;
    public TextMeshProUGUI co2EmissionPerMonthText;
    public TextMeshProUGUI co2AbsorptionPerMonthText;
    public TextMeshProUGUI wasteProductionPerMonthText;
    public TextMeshProUGUI wasteDestructionPerMonthText;
    
    public string euros = " €";
    public string energyUnit = " kWh";
    public string co2Unit = " kg";
    public string wasteUnit = " tonnes";
    public string greenSpacesUnit = " m²";
    public string percentUnit = " %";
    public string eurosPerMonth = " €/month";
    public string energyUnitPerMonth = " kWh/month";
    public string co2UnitPerMonth = " kg/month";
    public string wasteUnitPerMonth = " tonnes/month";
    
    public void UpdateTime(DateTime currentTime)
    {
        TimeText.text = currentTime.ToString("yyyy-MM-dd-HH:mm:ss");
    }
    
    public void UpdateFromStats(StatManager statManager)
    {
        // Global stats
        overallEcologyRateTextText.text = StatUtils.ConvertPercentToText(statManager.globalStats.overallEcologyRate) + percentUnit;
        overallSocietyRateText.text = StatUtils.ConvertPercentToText(statManager.globalStats.overallSocietyRate) + percentUnit;
        currentMoneyInBankText.text = StatUtils.ConvertFloatToText(statManager.globalStats.currentMoneyInBank) + euros;
        currentEnergyInStockText.text = StatUtils.ConvertFloatToText(statManager.globalStats.currentEnergyInStock) + energyUnit;
        currentEmittedCo2Text.text = StatUtils.ConvertFloatToText(statManager.globalStats.currentEmittedCo2) + co2Unit;
        currentWasteProducedText.text = StatUtils.ConvertFloatToText(statManager.globalStats.currentWasteProduced) + wasteUnit;
        
        // Citizens stats
        maxPopSizeText.text = statManager.citizensGestion.totalHouseholds.ToString();
        totalCitizensText.text = statManager.citizensGestion.totalCitizens.ToString();
        
        healthText.text = StatUtils.ConvertPercentToText(statManager.citizensGestion.citizensStats.health) + percentUnit;
        happinessText.text = StatUtils.ConvertPercentToText(statManager.citizensGestion.citizensStats.happiness) + percentUnit;
        sensibilisationText.text = StatUtils.ConvertPercentToText(statManager.citizensGestion.citizensStats.sensibilisation) + percentUnit;
        // acceptationText.text = StatUtils.ConvertPercentToText(statManager.citizensGestion.citizensStats.acceptation) + percentUnit;
        
        // dailyTransportsUsersText.text = statManager.citizensGestion.dailyTransportsUsers;
        
        // Objects stats
        currentGreenSpacesText.text = statManager.objectsStats.greenSpaces.ToString() + greenSpacesUnit;
        
        profitsPerMonthText.text = StatUtils.ConvertFloatToText(statManager.dashboardStats.profitsPerMonth) + eurosPerMonth;
        lossesPerMonthText.text = StatUtils.ConvertFloatToText(statManager.dashboardStats.lossesPerMonth) + eurosPerMonth;
        energyProductionPerMonthText.text = StatUtils.ConvertFloatToText(statManager.dashboardStats.energyProductionPerMonth) + energyUnitPerMonth;
        energyConsumptionPerMonthText.text = StatUtils.ConvertFloatToText(statManager.dashboardStats.energyConsumptionPerMonth) + energyUnitPerMonth;
        co2EmissionPerMonthText.text = StatUtils.ConvertFloatToText(statManager.dashboardStats.co2EmissionPerMonth) + co2UnitPerMonth;
        co2AbsorptionPerMonthText.text = StatUtils.ConvertFloatToText(statManager.dashboardStats.co2AbsorptionPerMonth) + co2UnitPerMonth;
        wasteProductionPerMonthText.text = StatUtils.ConvertFloatToText(statManager.dashboardStats.wasteProductionPerMonth) + wasteUnitPerMonth;
        wasteDestructionPerMonthText.text = StatUtils.ConvertFloatToText(statManager.dashboardStats.wasteDestructionPerMonth) + wasteUnitPerMonth;
    }
}
