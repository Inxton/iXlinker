﻿using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {

        private PdoViewModel CreatePdo(EtherCATSlavePdo pdo,BoxViewModel boxViewModel)
        {
            PdoViewModel pdoViewModel = FillPdoData(pdo, boxViewModel);

            ObservableCollection<PdoEntryViewModel> pdoEntriesUnstructured = GetAllPdoEntriesUnstructured(pdo, pdoViewModel);
            pdoViewModel.PdoEntriesUnstructured = pdoEntriesUnstructured;

            //ObservableCollection<PdoEntryViewModel> pdoEntriesStructured = GetAllPdoEntriesStructured(pdo, pdoViewModel, pdoEntriesUnstructured);
            //pdoViewModel.PdoEntriesStructured = pdoEntriesStructured;


            ObservableCollection<PdoEntryViewModel> pdoEntriesStructured = CreateStructuresForPdoEntries(pdo, pdoViewModel, pdoEntriesUnstructured);
            pdoViewModel.PdoEntriesStructured = pdoEntriesStructured;

            //MappableObject mapableObject = GetAllPdoEntriesAsOneStructure(pdoViewModel, pdoEntriesStructured);
            //pdoViewModel.MapableObject = mapableObject;

            GetAllPdoEntriesAsOneStructureAndCreateMapings(pdoEntriesStructured , ref pdoViewModel );

            return pdoViewModel;
        }
    }
}
