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
    
    public void UpdateFromStats(GlobalStats globalStats, Stat objectsStats, int maxPopSize, int totalCitizens, float acceptation, string dailyTransportsUsers)
    {
        overallEcologyRateTextText.text = StatUtils.ConvertPercentToText(globalStats.overallEcologyRate) + percentUnit;
        overallSocietyRateText.text = StatUtils.ConvertPercentToText(globalStats.overallSocietyRate) + percentUnit;
        
        currentMoneyInBankText.text = StatUtils.ConvertFloatToText(globalStats.currentMoneyInBank) + euros;
        currentEnergyInStockText.text = StatUtils.ConvertFloatToText(globalStats.currentEnergyInStock) + energyUnit;
        currentEmittedCo2Text.text = StatUtils.ConvertFloatToText(globalStats.currentEmittedCo2) + co2Unit;
        currentWasteProducedText.text = StatUtils.ConvertFloatToText(globalStats.currentWasteProduced) + wasteUnit;
        currentGreenSpacesText.text = StatUtils.ConvertNumberToText(objectsStats.greenSpaces) + greenSpacesUnit;
        
        maxPopSizeText.text = StatUtils.ConvertNumberToText(maxPopSize);
        totalCitizensText.text = StatUtils.ConvertNumberToText(totalCitizens);
        
        healthText.text = StatUtils.ConvertPercentToText(objectsStats.health) + percentUnit;
        happinessText.text = StatUtils.ConvertPercentToText(objectsStats.happiness) + percentUnit;
        sensibilisationText.text = StatUtils.ConvertPercentToText(objectsStats.sensibilisation) + percentUnit;
        acceptationText.text = StatUtils.ConvertPercentToText(acceptation) + percentUnit;
        
        dailyTransportsUsersText.text = dailyTransportsUsers;
        
        profitsPerMonthText.text = StatUtils.ConvertFloatToText(objectsStats.profitsPerMonth) + eurosPerMonth;
        lossesPerMonthText.text = StatUtils.ConvertFloatToText(objectsStats.lossesPerMonth) + eurosPerMonth;
        energyProductionPerMonthText.text = StatUtils.ConvertFloatToText(objectsStats.energyProductionPerMonth) + energyUnitPerMonth;
        energyConsumptionPerMonthText.text = StatUtils.ConvertFloatToText(objectsStats.energyConsumptionPerMonth) + energyUnitPerMonth;
        co2EmissionPerMonthText.text = StatUtils.ConvertFloatToText(objectsStats.co2EmissionPerMonth) + co2UnitPerMonth;
        co2AbsorptionPerMonthText.text = StatUtils.ConvertFloatToText(objectsStats.co2AbsorptionPerMonth) + co2UnitPerMonth;
        wasteProductionPerMonthText.text = StatUtils.ConvertFloatToText(objectsStats.wasteProductionPerMonth) + wasteUnitPerMonth;
        wasteDestructionPerMonthText.text = StatUtils.ConvertFloatToText(objectsStats.wasteDestructionPerMonth) + wasteUnitPerMonth;
    }
}
