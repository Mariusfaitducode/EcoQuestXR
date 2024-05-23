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
    
    public void UpdateFromStats(GameStats gameStats)
    {
        totalMoneyText.text = StatUtils.ConvertNumberToText(gameStats.currentMoneyInBank);
        totalEnergyText.text = StatUtils.ConvertNumberToText(gameStats.currentEnergyInStock);
        ecologyText.text = StatUtils.ConvertNumberToText(gameStats.overallEcologyRate);
        populationAcceptationText.text = StatUtils.ConvertNumberToText(gameStats.overallPopulationAcceptationRate);
        
        profitsText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.profits);
        lossesText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.losses);
        constructionCostText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.constructionCost);
        destructionCostText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.destructionCost);
        
        
        energyProductionText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.energyProduction);
        energyConsumptionText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.energyConsumption);
        energyConstructionCostText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.energyConstructionCost);
        energyDestructionCostText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.energyDestructionCost);
        
        
        biodiversityText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.biodiversity);
        airQualityText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.airQuality);
        groundQualityText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.groundQuality);
        
        populationSensibilisationText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.sensibilisation);
        populationSizeText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.size);
        populationHealthText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.health);
        populationHappinessText.text = StatUtils.ConvertNumberToText(gameStats.globalStats.happiness);
    }
}
