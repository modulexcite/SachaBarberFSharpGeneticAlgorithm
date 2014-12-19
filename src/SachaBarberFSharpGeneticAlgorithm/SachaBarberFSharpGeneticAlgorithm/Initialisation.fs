namespace FsIOCWindow
module Initialisation = 

    open Deedle
    open System
    open System.Configuration
    open FSharp.Data

    type initialPopulationDataFromCsv = CsvProvider<"InitialPopulation.csv">

    let createInitialPopulationFromCsvUsingCsvProvider() = 
        let exePath = System.Reflection.Assembly.GetEntryAssembly().Location
        let path = exePath.Substring(0,exePath.LastIndexOf(@"\") + 1) + "InitialPopulation.csv"
        let populationFromCsv = initialPopulationDataFromCsv.Load(path)
        populationFromCsv.Rows 
            |> Seq.map (fun row -> 
                { 
                    FaceGene=Convert.ToDouble(row.FaceGene);
                    LeftEyeGene=Convert.ToDouble(row.LeftEyeGene);
                    RightEyeGene=Convert.ToDouble(row.RightEyeGene);
                    NoseGene=Convert.ToDouble(row.NoseGene);
                    LeftBoneTopGene=Convert.ToDouble(row.LeftBoneTopGene);
                    RightBoneTopGene=Convert.ToDouble(row.RightBoneTopGene);
                    TeethGene=Convert.ToDouble(row.TeethGene);
                    LeftBoneBottomGene=Convert.ToDouble(row.LeftBoneBottomGene);
                    RightBoneBottomGene=Convert.ToDouble(row.RightBoneBottomGene);
                })

    let createInitialPopulationFromCsvUsingDeedle() = 
        let exePath = System.Reflection.Assembly.GetEntryAssembly().Location
        let path = exePath.Substring(0,exePath.LastIndexOf(@"\") + 1) + "InitialPopulation.csv"
        let testCsv = Frame.ReadCsv(path)
        let loadedPopulation  = 
            testCsv 
            |> Frame.mapRowValues (fun row -> 
                { 
                    FaceGene=Convert.ToDouble(row?FaceGene);
                    LeftEyeGene=Convert.ToDouble(row?LeftEyeGene);
                    RightEyeGene=Convert.ToDouble(row?RightEyeGene);
                    NoseGene=Convert.ToDouble(row?NoseGene);
                    LeftBoneTopGene=Convert.ToDouble(row?LeftBoneTopGene);
                    RightBoneTopGene=Convert.ToDouble(row?RightBoneTopGene);
                    TeethGene=Convert.ToDouble(row?TeethGene);
                    LeftBoneBottomGene=Convert.ToDouble(row?LeftBoneBottomGene);
                    RightBoneBottomGene=Convert.ToDouble(row?RightBoneBottomGene);
                })
        loadedPopulation.Values


    let GetUsageMessage() =
        "SachaBarberFSharpFinalProject must run with either 'Deedle' 
                    or 'CSVProvider'\r\nfor the InitialPopulationLoadingStrategy App.Config setting"

    let DoInitialPopulationLoadingStrategy() =
        let initialPopulationProviderType = 
            ConfigurationManager.AppSettings.Item("InitialPopulationLoadingStrategy")
        match initialPopulationProviderType with
            | "Deedle" -> createInitialPopulationFromCsvUsingDeedle()
            | "CSVProvider" ->   createInitialPopulationFromCsvUsingCsvProvider()
            | _ -> failwith  "Unknown InitialPopulationLoadingStrategy" 


    let printPopulation(population) =
        for popItem in population do
            printfn "%O" popItem



    let InitialiseIOC() =
        IOCManager.Instance.Container.Register<IPopulationInitialiser>(
            typeof<PopulationInitialiser>, Singleton)
        ()

