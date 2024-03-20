using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities.Address;
using RestaurantManagement.Data.RequestModels;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Business.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly DataContext _context;
        public AddressService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> AddNewCountry(CountryRequestModel countryRequest)
        {
            if (countryRequest == null) return false;

            var country = new ListOfCountries()
            {
                CountryCode = countryRequest.CountryCode,
                CountryNameEN = countryRequest.CountryNameEN,
                CountryNameVNI = countryRequest.CountryNameVNI,
            };
            _context.Countries.Add(country);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ListOfCountries>> GetListOfCountries()
        {
            return await _context.Countries.ToListAsync();
        }
        public async Task<List<Districts>> GetListDistrictsByProvince(long provinceId)
        {
            var province = await _context.Provinces.Include(x => x.ListDistrict).FirstOrDefaultAsync(x => x.Id == provinceId);
            if (province == null)
                return new List<Districts>();

            return province.ListDistrict.Select(x => new Districts
            {
                Id = x.Id,
                DistrictCode = x.DistrictCode,
                DistrictNameEN = x.DistrictNameEN,
                DistrictNameVNI = x.DistrictNameVNI,
            }).ToList();
        }

        public async Task<List<Provinces>> GetListProvinces(long? countryId)
        {
            if (countryId != null)
                return await _context.Provinces.Include(x => x.Country).Where(x => x.Country!.Id == countryId).ToListAsync();
            return await _context.Provinces.ToListAsync();
        }

        public async Task<List<Wards>> GetListWardsByDistrict(long districtId)
        {
            var district = await _context.Districts.Include(x => x.ListWards).FirstOrDefaultAsync(x => x.Id == districtId);
            if (district == null)
                return new List<Wards>();

            return district.ListWards.Select(x => new Wards
            {
                Id = x.Id,
                WardCode = x.WardCode,
                WardNameEN = x.WardNameEN,
                WardNameVNI = x.WardNameVNI,
            }).ToList();
        }

        public async Task<bool> UploadProvincesDistrictsWardFromFileExcel(Enums.Classification classification, IFormFile file)
        {
            if ((classification == Enums.Classification.Provinces && _context.Provinces.Any())
                || (classification == Enums.Classification.Districts && _context.Districts.Any())
                    || (classification == Enums.Classification.Wards && _context.Wards.Any()))
                throw new ValidationException("The data is already available, so there is no need for an update.");

            int firstCol = 0;
            int firstRow = 0;
            int lastCol = 0;
            int lastRow = 0;

            if (file.FileName.Split(".").Last() != "xls" && file.FileName.Split(".").Last() != "xlsx")
                throw new ValidationException("Please send an excel file to upload");

            var ms = new MemoryStream();
            file.CopyTo(ms);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(ms))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[(int)classification - 1];

                var start = sheet.Dimension.Start;
                var end = sheet.Dimension.End;

                firstCol = start.Column;
                firstRow = start.Row;

                lastCol = end.Column;
                lastRow = end.Row;

                if (classification == Enums.Classification.Provinces)
                {
                    List<Provinces> listProvince = new List<Provinces>();
                    for (int r = firstRow + 1; r <= lastRow; r++)
                    {
                        var provinceCode = sheet.Cells[r, firstCol].Value?.ToString();
                        if (string.IsNullOrEmpty(provinceCode)) break;
                        var countryId = long.Parse(sheet.Cells[r, firstCol + 4].Value?.ToString()!);
                        Provinces model = new Provinces
                        {
                            ProvinceCode = provinceCode,
                            ProvinceNameVNI = sheet.Cells[r, firstCol + 1].Value?.ToString()!,
                            ProvinceNameEN = sheet.Cells[r, firstCol + 2].Value?.ToString() ?? "",
                            Level = sheet.Cells[r, firstCol + 3].Value.ToString() ?? "",
                            Country = _context.Countries.FirstOrDefault(x => x.Id == countryId)!
                        };

                        listProvince.Add(model);
                    }
                    await _context.Provinces.AddRangeAsync(listProvince);
                }

                if (classification == Enums.Classification.Districts)
                {
                    List<Districts> listDistrict = new List<Districts>();
                    for (int r = firstRow + 1; r <= lastRow; r++)
                    {
                        var districtCode = sheet.Cells[r, firstCol].Value?.ToString();
                        if (string.IsNullOrEmpty(districtCode)) break;
                        var provinceCode = sheet.Cells[r, firstCol + 4].Value?.ToString();
                        Districts model = new Districts
                        {
                            DistrictCode = districtCode,
                            DistrictNameVNI = sheet.Cells[r, firstCol + 1].Value?.ToString()!,
                            DistrictNameEN = sheet.Cells[r, firstCol + 2].Value?.ToString() ?? "",
                            Level = sheet.Cells[r, firstCol + 3].Value?.ToString() ?? "",
                            Province = _context.Provinces.FirstOrDefault(x => x.ProvinceCode == provinceCode)!,
                        };
                        listDistrict.Add(model);
                    }
                    await _context.Districts.AddRangeAsync(listDistrict);
                }

                if(classification == Enums.Classification.Wards)
                {
                    List<Wards> listWards = new List<Wards>();
                    for(int r = firstRow + 1; r <= lastRow; r++)
                    {
                        var wardCode = sheet.Cells[r, firstCol].Value?.ToString();
                        if(string.IsNullOrEmpty(wardCode)) break;
                        var districtCode = sheet.Cells[r, firstCol + 4].Value?.ToString();
                        var provinceCode = sheet.Cells[r, firstCol + 6].Value?.ToString();
                        Wards model = new Wards
                        {
                            WardCode = wardCode,
                            WardNameVNI = sheet.Cells[r, firstCol + 1].Value?.ToString()!,
                            WardNameEN = sheet.Cells[r, firstCol + 2].Value?.ToString() ?? "",
                            Level = sheet.Cells[r, firstCol + 3].Value.ToString() ?? "",
                            District = _context.Districts.FirstOrDefault(x => x.DistrictCode == districtCode)!,
                            Province = _context.Provinces.FirstOrDefault(x => x.ProvinceCode == provinceCode)!,
                        };
                        listWards.Add(model);
                    }
                    await _context.Wards.AddRangeAsync(listWards);
                }

                return _context.SaveChanges() > 0;
            }
        }
    }
}
