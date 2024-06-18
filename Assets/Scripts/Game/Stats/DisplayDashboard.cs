using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashboardPopSize
{
    public int maxPopSize = 0;
    public int totalCitizens = 0;
}
public class DashboardTransportsRatio
{
    public float percentCar = 0;
    public float percentWalk = 0;
    public float percentBike = 0;
    public float percentBus = 0;
    public float percentTaxi = 0;
}
public class DashboardAreaStats
{
    public int greenSpaces = 0;
    public float monthlyWasteDifference = 0;
    public float monthlyCo2Difference = 0;
}
public class DashboardUniqueStats
{
    public float currentValue = 0;
    public float maxValue = 0;
    public float monthlyValue = 0;
    public float ratio = 0;
}


public class DisplayDashboard : MonoBehaviour
{
    // All stored values
    // Top panel
    private DateTime currentTime;
    private DashboardUniqueStats money = new DashboardUniqueStats();
    private DashboardUniqueStats energy = new DashboardUniqueStats();
    
    // Ecology panel
    private DashboardUniqueStats ecologyRate = new DashboardUniqueStats();
    private DashboardUniqueStats co2 = new DashboardUniqueStats();
    private DashboardUniqueStats waste = new DashboardUniqueStats();
    private DashboardUniqueStats greenSpaces = new DashboardUniqueStats();
    
    // Society panel
    private DashboardUniqueStats societyRate = new DashboardUniqueStats();
    private DashboardUniqueStats health = new DashboardUniqueStats();
    private DashboardUniqueStats happiness = new DashboardUniqueStats();
    private DashboardUniqueStats sensibilisation = new DashboardUniqueStats();
    
    // Citizens panel
    private DashboardPopSize popSize = new DashboardPopSize();
    private DashboardTransportsRatio transportsRatio = new DashboardTransportsRatio();
    
    // Area panel
    private DashboardAreaStats industryStats = new DashboardAreaStats();
    private DashboardAreaStats energyStats = new DashboardAreaStats();
    private DashboardAreaStats cityStats = new DashboardAreaStats();
    private DashboardAreaStats agricultureStats = new DashboardAreaStats();
  
    // Textes
    // Top panel
    public TextMeshProUGUI currentTimeText;
    
    public TextMeshProUGUI currentMoneyInBankText;
    public TextMeshProUGUI monthlyMoneyInBankText;
    public Slider moneySlider;
    
    public TextMeshProUGUI currentEnergyInStockText;
    public TextMeshProUGUI monthlyEnergyInStockText;
    public Slider energySlider;
    
    // Ecology panel
    public Slider overallEcologyRateText;
    
    public TextMeshProUGUI currentEmittedCo2Text;
    public TextMeshProUGUI monthlyEmittedCo2Text;
    public Slider co2Slider;
    
    public TextMeshProUGUI currentWasteProducedText;
    public TextMeshProUGUI monthlyWasteProducedText;
    public Slider wasteSlider;
    
    public TextMeshProUGUI currentGreenSpacesText;
    public Slider greenSpacesSlider;
    
    // Society panel
    public Slider overallSocietyRateText;
    
    public Slider healthSlider;
    public Slider happinessSlider;
    public Slider sensibilisationSlider;
    
    // Citizens panel
    public TextMeshProUGUI maxPopSizeText;
    public TextMeshProUGUI totalCitizensText;
    
    public TextMeshProUGUI dailyCarUsersText;
    public TextMeshProUGUI dailyWalkUsersText;
    public TextMeshProUGUI dailyBikeUsersText;
    public TextMeshProUGUI dailyBusUsersText;
    public TextMeshProUGUI dailyTaxiUsersText;
    
    // Area panel
    public TextMeshProUGUI industryGreenSpacesText;
    public TextMeshProUGUI industryMonthlyWasteDifferenceText;
    public TextMeshProUGUI industryMonthlyCo2DifferenceText;
    
    public TextMeshProUGUI energyGreenSpacesText;
    public TextMeshProUGUI energyMonthlyWasteDifferenceText;
    public TextMeshProUGUI energyMonthlyCo2DifferenceText;
    
    public TextMeshProUGUI cityGreenSpacesText;
    public TextMeshProUGUI cityMonthlyWasteDifferenceText;
    public TextMeshProUGUI cityMonthlyCo2DifferenceText;
    
    public TextMeshProUGUI agricultureGreenSpacesText;
    public TextMeshProUGUI agricultureMonthlyWasteDifferenceText;
    public TextMeshProUGUI agricultureMonthlyCo2DifferenceText;
    
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
        this.currentTime = currentTime;
        currentTimeText.text = currentTime.ToString("dd/MM/yyyy - HH:mm:ss");
    }
    
    public void UpdateDashboardStats(ClassStats stats, CitizensGestion citizensGestion, MaxStats maxStats)
    {
        // Top panel
        // money
        money.currentValue = stats.currentGlobalStats.money;
        money.maxValue = maxStats.globalStats.money;
        money.monthlyValue = stats.monthlyGlobalStats.money;
        money.ratio = money.currentValue / money.maxValue;
        
        // energy
        energy.currentValue = stats.currentGlobalStats.energy;
        energy.maxValue = maxStats.globalStats.energy;
        energy.monthlyValue = stats.monthlyGlobalStats.energy;
        energy.ratio = energy.currentValue / energy.maxValue;
        
        // Ecology panel
        // ecologyRate
        ecologyRate.currentValue = stats.overallEcologyRate;
        
        // co2
        co2.currentValue = stats.currentEcologyStats.co2;
        co2.maxValue = maxStats.globalEcologyStats.co2;
        co2.monthlyValue = stats.monthlyEcologyStats.co2;
        co2.ratio = co2.currentValue / co2.maxValue;
        
        // waste
        waste.currentValue = stats.currentEcologyStats.waste;
        waste.maxValue = maxStats.globalEcologyStats.waste;
        waste.monthlyValue = stats.monthlyEcologyStats.waste;
        waste.ratio = waste.currentValue / waste.maxValue;
        
        // greenSpaces
        greenSpaces.currentValue = stats.currentEcologyStats.greenSpaces;
        greenSpaces.maxValue = maxStats.globalEcologyStats.greenSpaces;
        greenSpaces.ratio = greenSpaces.currentValue / greenSpaces.maxValue;
        
        // Society panel
        // societyRate
        societyRate.currentValue = stats.overallSocietyRate;
        
        // health
        health.ratio = citizensGestion.citizensStats.health;
        happiness.ratio = citizensGestion.citizensStats.happiness;
        sensibilisation.ratio = citizensGestion.citizensStats.sensibilisation;
        
        // Citizens panel
        popSize.maxPopSize = citizensGestion.totalHouseholds;
        popSize.totalCitizens = citizensGestion.totalCitizens;
        
        transportsRatio.percentCar = (float)citizensGestion.transportModes.Find(transportMode => transportMode.transportModeType == TransportModeType.Car).dailyUsers / popSize.totalCitizens;
        transportsRatio.percentWalk = (float)citizensGestion.transportModes.Find(transportMode => transportMode.transportModeType == TransportModeType.Walk).dailyUsers / popSize.totalCitizens;
        transportsRatio.percentBike = (float)citizensGestion.transportModes.Find(transportMode => transportMode.transportModeType == TransportModeType.Bike).dailyUsers / popSize.totalCitizens;
        transportsRatio.percentBus = (float)citizensGestion.transportModes.Find(transportMode => transportMode.transportModeType == TransportModeType.Bus).dailyUsers / popSize.totalCitizens;
        transportsRatio.percentTaxi = (float)citizensGestion.transportModes.Find(transportMode => transportMode.transportModeType == TransportModeType.Taxi).dailyUsers / popSize.totalCitizens;
        
        // Area panel
        // industryStats
        ClassEcologyStats industryS = stats.ecologyStatsByArea.Find(areaStats => areaStats.areaType == AreaType.Industry);
        industryStats.greenSpaces = industryS.greenSpaces;
        industryStats.monthlyWasteDifference = industryS.waste;
        industryStats.monthlyCo2Difference = industryS.co2;
        
        // energyStats
        ClassEcologyStats energyS = stats.ecologyStatsByArea.Find(areaStats => areaStats.areaType == AreaType.Energy);
        energyStats.greenSpaces = energyS.greenSpaces;
        energyStats.monthlyWasteDifference = energyS.waste;
        energyStats.monthlyCo2Difference = energyS.co2;
        
        // cityStats
        ClassEcologyStats cityS = stats.ecologyStatsByArea.Find(areaStats => areaStats.areaType == AreaType.City);
        cityStats.greenSpaces = cityS.greenSpaces;
        cityStats.monthlyWasteDifference = cityS.waste;
        cityStats.monthlyCo2Difference = cityS.co2;
        
        // agricultureStats
        ClassEcologyStats agricultureS = stats.ecologyStatsByArea.Find(areaStats => areaStats.areaType == AreaType.Agriculture);
        agricultureStats.greenSpaces = agricultureS.greenSpaces;
        agricultureStats.monthlyWasteDifference = agricultureS.waste;
        agricultureStats.monthlyCo2Difference = agricultureS.co2;
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Top panel
        // money
        currentTimeText.text = currentTime.ToString("dd/MM/yyyy");

        currentMoneyInBankText.text = StatUtils.ConvertFloatToText(money.currentValue) + euros;
        monthlyMoneyInBankText.text = StatUtils.ConvertFloatToText(money.monthlyValue) + eurosPerMonth;
        moneySlider.value = money.ratio;

        // *1000 car passe de kWh à Wh
        currentEnergyInStockText.text = StatUtils.ConvertFloatToText(energy.currentValue*1000) + energyUnit;
        monthlyEnergyInStockText.text = StatUtils.ConvertFloatToText(energy.monthlyValue*1000) + energyUnitPerMonth;
        energySlider.value = energy.ratio;

        // Ecology panel
        // ecologyRate
        overallEcologyRateText.value = ecologyRate.currentValue;

        // co2
        currentEmittedCo2Text.text = StatUtils.ConvertFloatToText(co2.currentValue) + co2Unit;
        monthlyEmittedCo2Text.text = StatUtils.ConvertFloatToText(co2.monthlyValue) + co2UnitPerMonth;
        co2Slider.value = co2.ratio;

        // waste
        currentWasteProducedText.text = StatUtils.ConvertFloatToText(waste.currentValue) + wasteUnit;
        monthlyWasteProducedText.text = StatUtils.ConvertFloatToText(waste.monthlyValue) + wasteUnitPerMonth;
        wasteSlider.value = waste.ratio;

        // greenSpaces
        currentGreenSpacesText.text = StatUtils.ConvertIntToText((int)greenSpaces.currentValue) + greenSpacesUnit;
        greenSpacesSlider.value = greenSpaces.ratio;

        // Society panel
        // societyRate
        overallSocietyRateText.value = societyRate.currentValue;

        // health
        healthSlider.value = health.ratio;
        happinessSlider.value = happiness.ratio;
        sensibilisationSlider.value = sensibilisation.ratio;

        // Citizens panel
        maxPopSizeText.text = popSize.maxPopSize.ToString();
        totalCitizensText.text = popSize.totalCitizens.ToString();

        dailyCarUsersText.text = StatUtils.ConvertPercentToText(transportsRatio.percentCar) + percentUnit;
        dailyWalkUsersText.text = StatUtils.ConvertPercentToText(transportsRatio.percentWalk) + percentUnit;
        dailyBikeUsersText.text = StatUtils.ConvertPercentToText(transportsRatio.percentBike) + percentUnit;
        dailyBusUsersText.text = StatUtils.ConvertPercentToText(transportsRatio.percentBus) + percentUnit;
        dailyTaxiUsersText.text = StatUtils.ConvertPercentToText(transportsRatio.percentTaxi) + percentUnit;

        // Area panel
        // industryStats
        industryGreenSpacesText.text = StatUtils.ConvertIntToText(industryStats.greenSpaces) + greenSpacesUnit;
        industryMonthlyWasteDifferenceText.text = StatUtils.ConvertFloatToText(industryStats.monthlyWasteDifference) + wasteUnitPerMonth;
        industryMonthlyCo2DifferenceText.text = StatUtils.ConvertFloatToText(industryStats.monthlyCo2Difference) + co2UnitPerMonth;

        // energyStats
        energyGreenSpacesText.text = StatUtils.ConvertIntToText(energyStats.greenSpaces) + greenSpacesUnit;
        energyMonthlyWasteDifferenceText.text = StatUtils.ConvertFloatToText(energyStats.monthlyWasteDifference) + wasteUnitPerMonth;
        energyMonthlyCo2DifferenceText.text = StatUtils.ConvertFloatToText(energyStats.monthlyCo2Difference) + co2UnitPerMonth;

        // cityStats
        cityGreenSpacesText.text = StatUtils.ConvertIntToText(cityStats.greenSpaces) + greenSpacesUnit;
        cityMonthlyWasteDifferenceText.text = StatUtils.ConvertFloatToText(cityStats.monthlyWasteDifference) + wasteUnitPerMonth;
        cityMonthlyCo2DifferenceText.text = StatUtils.ConvertFloatToText(cityStats.monthlyCo2Difference) + co2UnitPerMonth;

        // agricultureStats
        agricultureGreenSpacesText.text = StatUtils.ConvertIntToText(agricultureStats.greenSpaces) + greenSpacesUnit;
        agricultureMonthlyWasteDifferenceText.text = StatUtils.ConvertFloatToText(agricultureStats.monthlyWasteDifference) + wasteUnitPerMonth;
        agricultureMonthlyCo2DifferenceText.text = StatUtils.ConvertFloatToText(agricultureStats.monthlyCo2Difference) + co2UnitPerMonth;
    }
}
