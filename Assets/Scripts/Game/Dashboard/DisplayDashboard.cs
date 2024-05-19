using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayDashboard : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI TimeText;
    
    public TextMeshProUGUI populationSizeText;
    public TextMeshProUGUI populationHealthText;
    public TextMeshProUGUI populationHappinessText;
    public TextMeshProUGUI populationSensibilisationText;
    public TextMeshProUGUI populationAcceptationText;
    
    public TextMeshProUGUI energyStockText;
    public TextMeshProUGUI energyProductionText;
    public TextMeshProUGUI energyConsumptionText;
    
    public TextMeshProUGUI ecologyText;
    public TextMeshProUGUI biodiversityText;
    
    public TextMeshProUGUI pollutionAirText;
    public TextMeshProUGUI pollutionGroundText;
    public TextMeshProUGUI pollutionNoiseText;
    public TextMeshProUGUI pollutionVisualText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void UpdateTime(DateTime currentTime)
    {
        TimeText.text = currentTime.ToString("yyyy-MM-dd-HH:mm:ss");
    }
    
    public void UpdateFromGameStats(GameStats gameStats)
    {
        moneyText.text = gameStats.money.ToString();
        
        populationSizeText.text = gameStats.populationSize.ToString();
        // populationHealthText.text = gameStats.populationHealth.ToString();
        // populationHappinessText.text = gameStats.populationHappiness.ToString();
        // populationSensibilisationText.text = gameStats.populationSensibilisation.ToString();
        // populationAcceptationText.text = gameStats.populationAcceptation.ToString();
        
        energyStockText.text = gameStats.energyStock.ToString();
        // energyProductionText.text = gameStats.energyProduction.ToString();
        // energyConsumptionText.text = gameStats.energyConsumption.ToString();
        
        ecologyText.text = gameStats.ecology.ToString();
        // biodiversityText.text = gameStats.biodiversity.ToString();
        
        pollutionAirText.text = gameStats.pollutionAir.ToString();
        // pollutionGroundText.text = gameStats.pollutionGround.ToString();
        // pollutionNoiseText.text = gameStats.pollutionNoise.ToString();
        // pollutionVisualText.text = gameStats.pollutionVisual.ToString();
    }
    
    // TODO : Remove this function and use UpdateFromGameStats instead when all stats are implemented
    public void InitialUpdate(GameStats gameStats)
    {
        moneyText.text = gameStats.money.ToString();
        
        populationSizeText.text = gameStats.populationSize.ToString();
        populationHealthText.text = gameStats.populationHealth.ToString();
        populationHappinessText.text = gameStats.populationHappiness.ToString();
        populationSensibilisationText.text = gameStats.populationSensibilisation.ToString();
        populationAcceptationText.text = gameStats.populationAcceptation.ToString();
        
        energyStockText.text = gameStats.energyStock.ToString();
        energyProductionText.text = gameStats.energyProduction.ToString();
        energyConsumptionText.text = gameStats.energyConsumption.ToString();
        
        ecologyText.text = gameStats.ecology.ToString();
        biodiversityText.text = gameStats.biodiversity.ToString();
        
        pollutionAirText.text = gameStats.pollutionAir.ToString();
        pollutionGroundText.text = gameStats.pollutionGround.ToString();
        pollutionNoiseText.text = gameStats.pollutionNoise.ToString();
        pollutionVisualText.text = gameStats.pollutionVisual.ToString();
    }
}
