using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayDashboard : MonoBehaviour
{
    public TextMeshProUGUI TimeText;
    
    public TextMeshProUGUI totalMoneyText;
    public TextMeshProUGUI profitsText;
    public TextMeshProUGUI lossesText;
    public TextMeshProUGUI constructionCostText;
    public TextMeshProUGUI destructionCostText;
    
    public TextMeshProUGUI totalEnergyText;
    public TextMeshProUGUI energyProductionText;
    public TextMeshProUGUI energyConsumptionText;
    public TextMeshProUGUI energyConstructionCostText;
    public TextMeshProUGUI energyDestructionCostText;
    
    public TextMeshProUGUI ecologyText;
    public TextMeshProUGUI biodiversityText;
    public TextMeshProUGUI airQualityText;
    public TextMeshProUGUI groundQualityText;
    
    public TextMeshProUGUI populationSensibilisationText;
    public TextMeshProUGUI populationSizeText;
    public TextMeshProUGUI populationHealthText;
    public TextMeshProUGUI populationHappinessText;
    public TextMeshProUGUI populationAcceptationText;
    
    public void UpdateTime(DateTime currentTime)
    {
        TimeText.text = currentTime.ToString("yyyy-MM-dd-HH:mm:ss");
    }
    
    public void UpdateFromStats(GlobalStats globalStats, Stat objectsStats)
    {
        totalMoneyText.text = StatUtils.ConvertNumberToText(globalStats.currentMoneyInBank);
        totalEnergyText.text = StatUtils.ConvertNumberToText(globalStats.currentEnergyInStock);
        ecologyText.text = StatUtils.ConvertNumberToText(globalStats.overallEcologyRate);
        populationAcceptationText.text = StatUtils.ConvertNumberToText(globalStats.overallPopulationAcceptationRate);
        
        profitsText.text = StatUtils.ConvertNumberToText(objectsStats.profits);
        lossesText.text = StatUtils.ConvertNumberToText(objectsStats.losses);
        constructionCostText.text = StatUtils.ConvertNumberToText(objectsStats.constructionCost);
        destructionCostText.text = StatUtils.ConvertNumberToText(objectsStats.destructionCost);
        
        energyProductionText.text = StatUtils.ConvertNumberToText(objectsStats.energyProduction);
        energyConsumptionText.text = StatUtils.ConvertNumberToText(objectsStats.energyConsumption);
        energyConstructionCostText.text = StatUtils.ConvertNumberToText(objectsStats.energyConstructionCost);
        energyDestructionCostText.text = StatUtils.ConvertNumberToText(objectsStats.energyDestructionCost);
        
        biodiversityText.text = StatUtils.ConvertNumberToText(objectsStats.biodiversity);
        airQualityText.text = StatUtils.ConvertNumberToText(objectsStats.airQuality);
        groundQualityText.text = StatUtils.ConvertNumberToText(objectsStats.groundQuality);
        
        populationSensibilisationText.text = StatUtils.ConvertPercentToText(objectsStats.sensibilisation);
        populationSizeText.text = StatUtils.ConvertNumberToText(objectsStats.size);
        populationHealthText.text = StatUtils.ConvertPercentToText(objectsStats.health);
        populationHappinessText.text = StatUtils.ConvertPercentToText(objectsStats.happiness);
    }
}
