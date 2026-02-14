- Các câu lệnh Git quan trọng:
git clone <URL>
git status : xem trạng thái file
git add .
git commit -m "Tên Đặt cho Message Commit"
git push
- Các câu lệnh với branch:
git branch -r : xem danh sách các branch
git branch : xem branch hiện tại cùng với danh sách
git checkout <tên branch>
git checkout master : quay về banch main || master
git push : nếu để nguyên vậy thì nó tự động push lên branch hiện tại của nó (xem "git status" để biết branch nào )
git push origin main : push lên branch main || master

- Cách hoàn tác việc Push Commit:
git revert <commit_id> : "commit_id" xem trên detail của commit đấy. hoặc sử dụng "git log --oneline || git log"
git push origin <ten_branch>|| main  : sau khi sửa commit xong thì "push" lại

- Xóa hoàn toàn Commit
git reset --hard <commit_id>
git push origin <ten_branch> --force

- Xóa thứ tự Commit:
HEAD = commit hiện tại
HEAD~1 = commit trước đó
HEAD~2 = 2 commit trước
Ví dụ: git reset --hard HEAD~1
