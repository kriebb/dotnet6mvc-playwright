using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnet6mvcEcommerce.Data;
using dotnet6mvcEcommerce.Models;

namespace dotnet6mvcEcommerce.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }


        #region 取得所有商品
        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();

            return View(products);
        }
        #endregion

        #region 明細頁面
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        #endregion

        #region 新增商品
        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Stock,ImageUrl,FormFile")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.FormFile != null)
                {
                    //保存文件名稱--保存到資料庫
                    product.ImageUrl = await SaveImage(product.FormFile);

                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        #endregion

        #region 編輯商品
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Stock,ImageUrl, FormFile")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //保存圖片
                    if(product.FormFile != null)
                    {
                        if(product.ImageUrl != null)
                        {
                            RemoveImage(product.ImageUrl);//刪除舊圖片
                        }
                        string fileName = await SaveImage(product.FormFile);
                        product.ImageUrl = fileName;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        #endregion

        #region 刪除商品
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                //刪除圖片
                RemoveImage(product.ImageUrl);
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Private

        //刪除圖片
        //參數：文件名稱
        //返回值：void, bool
        private void RemoveImage(string? fileName)
        {
            if(fileName != null)
            {
                //獲取文件路徑
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);
                //刪除圖片
                System.IO.File.Delete(filePath);
            }


        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        private async Task<string> SaveImage(IFormFile formFile)
        {
            //文件名稱處理(不能有重複名稱)
            //1.GUID
            //2.把文件名稱用時間來保存
            string fileName = Guid.NewGuid().ToString() + ".jpg";//(可取得檔案副檔名去做附加，這邊統一改jpg)

            //保存位置
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

            //保存文件
            using (var stream = System.IO.File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
            }
            return fileName;
        }

        private bool ProductExists(int id)
        {
            //?., a?.b,如果a為空null,就不會計算b,就直接返回null
            bool? result = _context.Products?.Any(e => e.Id == id);
            return result.GetValueOrDefault();
        }
        #endregion


    }
}
