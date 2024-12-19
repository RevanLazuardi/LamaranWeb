using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LamaranWeb.Models;
using LamaranWeb.Repositories;
using System.IO;

namespace LamaranWeb.Controllers
{
    public class ApplicantsController : Controller
    {
        private readonly IApplicantRepository _repository;
        private readonly IMapper _mapper;

        public ApplicantsController(IApplicantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var applicants = await _repository.GetAllAsync();
            return View(applicants);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ApplicantDTO applicantDTO)
        {
            if (ModelState.IsValid)
            {
                var applicant = _mapper.Map<Applicant>(applicantDTO);
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Buat folder jika belum ada
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Jika ada file CV
                if (applicantDTO.CV != null)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(applicantDTO.CV.FileName);
                    var filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await applicantDTO.CV.CopyToAsync(stream);
                    }

                    applicant.CVName = uniqueFileName;
                }

                applicant.CreatedAt = DateTime.Now;

                await _repository.AddAsync(applicant);

                return RedirectToAction(nameof(Index));
            }
            return View(applicantDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var applicant = await _repository.GetByIdAsync(id);
            if (applicant == null)
                return NotFound();

            var applicantDTO = _mapper.Map<ApplicantDTO>(applicant); // mapping Applicant ke ApplicantDTO

            return View(applicantDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ApplicantDTO applicantDTO)
        {
            if (ModelState.IsValid)
            {
                var existingApplicant = await _repository.GetByIdAsync(id);
                if (existingApplicant == null)
                    return NotFound();

                _mapper.Map(applicantDTO, existingApplicant); // mapping ApplicantDTO ke Applicant

                if (applicantDTO.CV != null)
                {
                    // Hapus file CV lama jika ada
                    if (!string.IsNullOrEmpty(existingApplicant.CVName))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", existingApplicant.CVName);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath); 
                        }
                    }

                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(applicantDTO.CV.FileName);
                    var filePath = Path.Combine(uploadFolder, uniqueFileName);

                    // Simpan file CV baru
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await applicantDTO.CV.CopyToAsync(stream);
                    }

                    existingApplicant.CVName = uniqueFileName;
                }

                existingApplicant.CreatedAt = DateTime.Now;

                await _repository.UpdateAsync(existingApplicant);

                return RedirectToAction(nameof(Index));
            }
            return View(applicantDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var applicant = await _repository.GetByIdAsync(id);
            if (applicant != null)
            {
                // Hapus file CV jika ada
                if (!string.IsNullOrEmpty(applicant.CVName))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", applicant.CVName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                await _repository.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
