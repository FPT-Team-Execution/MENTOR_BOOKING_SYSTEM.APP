using ClosedXML.Excel;
using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Enums;
using MBS.Repositories.Interfaces;
using MBS.Services.Services.Interfaces;
using Position = MBS.BusinessObject.Entities.Position;

namespace MBS.Services.Services.Implements;

public class ProjectExcelService : IFileService<Project>
{
    private readonly IPositionRepository _positionRepository;
    private readonly IGroupRepository _groupRepository;

    public ProjectExcelService(IPositionRepository positionRepository, IGroupRepository groupRepository)
    {
        _positionRepository = positionRepository;
        _groupRepository = groupRepository;
    }

    public IEnumerable<Project> Import(string filePath, string sheetName)
    {
        List<Project> list = new List<Project>();
        Type typeOfObject = typeof(Project);

        using (IXLWorkbook workbook = new XLWorkbook(filePath))
        {
            //get target sheet
            var workSheet = workbook.Worksheets.FirstOrDefault(w => w.Name == sheetName);
            if (workSheet == null)
                return list;

            var properties = typeOfObject.GetProperties();
            var columns = workSheet.FirstRow().Cells().Select((v, i) => new { Value = v.Value, Index = i + 1 });
            foreach (IXLRow row in workSheet.RowsUsed().Skip(1))

            {
                Project obj = (Project)Activator.CreateInstance(typeof(Project));

                foreach (var prop in properties)
                {
                    int colIndex = columns.SingleOrDefault(c => c.Value.ToString() == prop.Name.ToString()).Index;
                    var val = row.Cell(colIndex).Value;
                    var type = prop.PropertyType;
                    prop.SetValue(obj, Convert.ChangeType(val, type));
                }

                list.Add(obj);
            }
        }

        return list;
    }

    public async Task<byte[]> Export(IEnumerable<Project> projects)
    {
        byte[] bytes = [];
        var positions = await _positionRepository.GetAllAsync();

        using var workBook = new XLWorkbook();
        //* Create sheet
        #region Create project sheet
        var workSheet = workBook.Worksheets.Add("Projects");
        var startRow = 1; //in Excel, row start from 1
        // workSheet.Cell(startRow, 1).Value = "ID";
        workSheet.Cell(startRow, 1).Value = "Title";
        workSheet.Cell(startRow, 2).Value = "Description";
        workSheet.Cell(startRow, 3).Value = "Due Date";
        workSheet.Cell(startRow, 4).Value = "Semester";
        workSheet.Cell(startRow, 5).Value = "Mentor Email";
        workSheet.Cell(startRow, 6).Value = "Status";
        workSheet.Cell(startRow, 7).Value = "Create By";
        workSheet.Cell(startRow, 8).Value = "Create On";
        workSheet.Cell(startRow, 9).Value = "Update By";
        workSheet.Cell(startRow, 10).Value = "Update On";
        //* Group Section
        workSheet.Cell(startRow, 11).Value = "Student Name";
        workSheet.Cell(startRow, 12).Value = "Position Name";

        #endregion

        #region Create HiddenSheet

        //Create hidden sheet to create dropdown cell
        var hiddenSheet = workBook.Worksheets.Add("HiddenSheet");
        hiddenSheet.Cell(1, 1).Value = "Position ID";
        hiddenSheet.Cell(1, 2).Value = "Position Name";
        //Add Default data to hidden sheet
        var positionsData = positions as Position[] ?? positions.ToArray();
        for (int i = 0; i < positionsData.Count(); i++)
        {
            hiddenSheet.Cell(i + 2, 1).Value = positionsData[i].Id.ToString(); // ID
            hiddenSheet.Cell(i + 2, 2).Value = positionsData[i].Name; // Tên hiển thị
        }

        // Hide "HiddenSheet"
        hiddenSheet.Hide();

        #endregion

        //* Check if no project, export empty template
        var projectsData = projects as Project[] ?? projects.ToArray();
        if (!projectsData.Any())
        {
            using (var memoryStream = new MemoryStream())
            {
                workBook.SaveAs(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return bytes;
        }

        //* Add data to Projects sheet
        var startDataRow = 2;
        var data = projectsData.ToArray();
        for (int pIndex = 0; pIndex < data.Length; pIndex++)
        {
            var groups = await _groupRepository.GetGroupByProjectIdAsync(data[pIndex].Id);
            var groupsArray = groups as Group[] ?? groups.ToArray();
            for (int i = 0; i <= groupsArray.Length; i++)
            {
                // workSheet.Cell(startDataRow + i, 1).Value = data[pIndex].Id.ToString();
                workSheet.Cell(startDataRow + i, 1).Value = data[pIndex].Title;
                workSheet.Cell(startDataRow + i, 2).Value = data[pIndex].Description;
                workSheet.Cell(startDataRow + i, 3).Value = data[pIndex].DueDate;
                workSheet.Cell(startDataRow + i, 4).Value = data[pIndex].Semester;
                workSheet.Cell(startDataRow + i, 5).Value = data[pIndex].Mentor.User.Email;
                workSheet.Cell(startDataRow + i, 6).Value = data[pIndex].Status.ToString();
                workSheet.Cell(startDataRow + i, 7).Value = data[pIndex].CreatedBy;
                workSheet.Cell(startDataRow + i, 8).Value = data[pIndex].CreatedOn;
                workSheet.Cell(startDataRow + i, 9).Value = data[pIndex].UpdatedBy;
                workSheet.Cell(startDataRow + i, 10).Value = data[pIndex].UpdatedOn;
                workSheet.Cell(startDataRow + i, 11).Value = groupsArray[i].Student.User.FullName;
                workSheet.Cell(startDataRow + i, 12).Value = groupsArray[i].Position.Name;

                // Add dropdown status to the cell (i, 6)
                var statusCell = workSheet.Cell(startDataRow + i, 6);
                var validation = statusCell.CreateDataValidation();
                validation.List(
                    $"{ProjectStatusEnum.Activated.ToString()},{ProjectStatusEnum.Deactivated.ToString()},{ProjectStatusEnum.Closed.ToString()}");
                validation.IgnoreBlanks = false;
                validation.InCellDropdown = true; // Enable dropdown display in the cell

                // Thêm dropdown cho cột "Position Name"
                var positionCell = workSheet.Cell(startDataRow + i, 12);
                positionCell.CreateDataValidation().List("HiddenSheet!$B$2:$B$" + (positionsData.Length + 1));
            }
        }
        //resize all columns to fit
        workSheet.Columns().AdjustToContents();

        using (var memoryStream = new MemoryStream())
        {
            workBook.SaveAs(memoryStream);
            bytes = memoryStream.ToArray();
        }

        return bytes;
    }
}