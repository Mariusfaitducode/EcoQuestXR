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
    
    public void UpdateFromStats(Stat stats)
    {
        moneyText.text = stats.price.ToString();
        
        populationSizeText.text = stats.population.ToString();
        populationHealthText.text = 0.ToString();
        populationHappinessText.text = 0.ToString();
        populationSensibilisationText.text = 0.ToString();
        populationAcceptationText.text = 0.ToString();
        
        energyStockText.text = stats.energy.ToString();
        energyProductionText.text = 0.ToString();
        energyConsumptionText.text = 0.ToString();
        
        ecologyText.text = stats.ecology.ToString();
        biodiversityText.text = 0.ToString();
        
        pollutionAirText.text = stats.pollution.ToString();
        pollutionGroundText.text = 0.ToString();
        pollutionNoiseText.text = 0.ToString();
        pollutionVisualText.text = 0.ToString();
    }
}
