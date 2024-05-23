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
    
    public void UpdateFromStats(Stat stats)
    {
        // totalMoneyText.text = StatUtils.ConvertNumberToText(stats.totalMoney);
        
        profitsText.text = StatUtils.ConvertNumberToText(stats.profits);
        lossesText.text = StatUtils.ConvertNumberToText(stats.losses);
        constructionCostText.text = StatUtils.ConvertNumberToText(stats.constructionCost);
        destructionCostText.text = StatUtils.ConvertNumberToText(stats.destructionCost);
        
        // totalEnergyText.text = StatUtils.ConvertNumberToText(stats.totalEnergy);
        energyProductionText.text = StatUtils.ConvertNumberToText(stats.energyProduction);
        energyConsumptionText.text = StatUtils.ConvertNumberToText(stats.energyConsumption);
        energyConstructionCostText.text = StatUtils.ConvertNumberToText(stats.energyConstructionCost);
        energyDestructionCostText.text = StatUtils.ConvertNumberToText(stats.energyDestructionCost);
        
        // ecologyText.text = StatUtils.ConvertNumberToText(stats.ecology);
        biodiversityText.text = StatUtils.ConvertNumberToText(stats.biodiversity);
        airQualityText.text = StatUtils.ConvertNumberToText(stats.airQuality);
        groundQualityText.text = StatUtils.ConvertNumberToText(stats.groundQuality);
        
        populationSensibilisationText.text = StatUtils.ConvertNumberToText(stats.sensibilisation);
        populationSizeText.text = StatUtils.ConvertNumberToText(stats.size);
        populationHealthText.text = StatUtils.ConvertNumberToText(stats.health);
        populationHappinessText.text = StatUtils.ConvertNumberToText(stats.happiness);
        // populationAcceptationText.text = StatUtils.ConvertNumberToText(stats.acceptation);
        
    }
}
