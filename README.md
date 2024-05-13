# BUILD WEB API BY ITSNgocNhan

Bài tập Lab được build lại từ ngày 22/04/2024 theo bài hướng dẫn pptx trong các bài giảng của thầy Tấn.

## Tools

```bash
VISUAL STUDIO
SQL
GIT
```

## Usage

```
git add .
git commit -m""
git push origin master
```
## LOG
### - Version 1  - 22/04/2024:
- Đã xây dựng được các Models - Controller cơ bản.
- Chưa fix được lỗi ở phương thức GetAll trong BookController.
### - Version 1.2  - 24/04/2024:
- Xây thêm các DTO.
- Chưa fix được các lỗi trong BookController.
### - Version 1.3  - 24/04/2024:
- Đã fix được phương thức GET trong BookController. Bao gồm: [HttpGet("get-all-books")] & [HttpGet][Route("get-book-by-id/{id}")]
- Chưa fix được các lỗi trong BookController.
### - Version 1.4  - 25/04/2024:
- Build lại các phương thức trong Controller.
- GET 202, POST - PUT - DELETE 404 =)))
- Build Repositories
- Change appDbContetxt => _dbContext
### - Version 1.5  - 13/05/2024:
- Xây dựng Controller Auhtor và Pushlisher
- Chưa thiết lập được Validate ở Model và Controller
## Contact

[Facebook](https://www.facebook.com/ItsNgocNhan/)

[Discord](ItsNgocNhan)
