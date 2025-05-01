#include <bits/stdc++.h>
#include <windows.h>
#include <conio.h>
using namespace std;

class Role{ // 角色基底
protected:
    string name;
    char icon;
    int x;
    int y;
public:
    Role(){
        name = "role";
        icon = 'R';
        x = 0;
        y = 0;
    }
    Role(string n, char i, int x, int y) : name(n), icon(i), x(x), y(y) {
    }

    int GetX(){ return this->x; };
    int GetY(){ return this->y; };
    void SetX(int x){ this->x = x; };
    void SetY(int y){ this->y = y; };

    string GetName(){ return name; };
    char GetIcon(){ return icon; };
};

class Snake : public Role{ // 蛇
private:
    char face; // 蛇的方向
    int rate; //蛇的速度
public:
    Snake() {
        Snake(0, 0);
    }
    Snake(int x, int y) : Role("snake", 'S', x, y) {
        face = 'L';
        rate = 1;
    }
    char GetFace(){ return face; };
    void SetFace(char f){ this->face = f; };
};

class Apple : public Role{ // 蘋果
public:
    Apple() : Role("apple", 'A', 0, 0) {
    }
    Apple(int x, int y) : Role("apple", 'A', x, y) {
    }
    void GotHit(){
        cout << name << ": got hit" << endl;
    }
};

class Map{
    /*
        v陣列紀錄地圖的使用狀況
        snakes紀錄蛇的整個身體
    */
private:
    int width;
    int height;
    Apple* apple;
    deque<Snake*> snakes;
    vector<vector<int>> v;
    COORD coord;
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    void SetCursorPosition(int x, int y) {
        coord.X = x;
        coord.Y = y;
        SetConsoleCursorPosition(hConsole, coord);
    }
    void HideCursor() {
        CONSOLE_CURSOR_INFO cursorInfo;
        GetConsoleCursorInfo(hConsole, &cursorInfo);
        cursorInfo.bVisible = false; // 隱藏游標
        SetConsoleCursorInfo(hConsole, &cursorInfo);
    }
    void Move(Role &role, char face){ //改變值
        int x = role.GetX();
        int y = role.GetY();
        switch(face){
            case 'U':
                if(y - 1 >= 0) role.SetY(role.GetY() + 1);
                break;
            case 'D':
                if(y + 1 < height) role.SetY(role.GetY() + 1);
                break;
            case 'L':
                if(x - 1 >= 0) role.SetX(role.GetX() - 1);
                break;
            case 'R':
                if(x + 1 < width) role.SetX(role.GetX() + 1);
                break;
        }
    }

    void Move(Role role, char face, int &ox, int &oy){ //只取值不改變
        int x = role.GetX();
        int y = role.GetY();
        switch(face){ //根據方向改變座標
            case 'U':
                if(y - 1 >= 0) y -= 1;
                break;
            case 'D':
                if(y + 1 < height) y += 1;
                break;
            case 'L':
                if(x - 1 >= 0) x -= 1;
                break;
            case 'R':
                if(x + 1 < width) x += 1;
                break;
        }
        ox = x; //傳回新的座標
        oy = y; //傳回新的座標
    }
public:
    Map(){
        Map(20, 20);
        // v = new vector<vector<int>>(height, vector<int>(width, 0));
        // apple = SetPos(new Apple(0, 0));
    }
    Map(int w, int h) : width(w), height(h) {
        HideCursor();
        v = vector<vector<int> >(height, vector<int>(width, 0));
        apple = new Apple();
        SetPos(*apple);
        snakes.push_back(new Snake(0, 0));
        SetPos(*snakes[0]);
        int tempx;
        int tempy;
        Move(*snakes[0], 'R', tempx, tempy); //測試用
        snakes.push_back(new Snake(tempx, tempy)); //測試用
        Move(*snakes[0], 'R', tempx, tempy); //測試用
        snakes.push_back(new Snake(tempx, tempy)); //測試用
        SetCursorPosition(apple->GetX(), apple->GetY()); //設定蘋果位置
    }

    ~Map(){
        delete apple;
        for(auto temp : snakes){
            delete temp;
        }
        //在偵錯時聽取copilot的建議，將記憶體釋放
    }

    void SetPos(Role &role){
        int x = rand() % width;
        int y = rand() % height;
        while(v[y][x] != 0){
            x = rand() % width;
            y = rand() % height;
        }
        v[y][x] = 1;
        role.SetX(x);
        role.SetY(y);
    }

    void Control(char ch){
        switch(ch){
            case 'w':
                snakes[snakes.size()]->SetFace('U');
                break;
            case 's':
                snakes[snakes.size()]->SetFace('D');
                break;
            case 'a':
                snakes[snakes.size()]->SetFace('L');
                break;
            case 'd':
                snakes[snakes.size()]->SetFace('R');
                break;
            default:
                break;
        }
        //Move(*snakes[0], snakes[0]->GetFace());
    }

    void Print(){ //印出地圖
        system("cls");
        //SetCursorPosition(apple->GetX(), apple->GetY()); //設定蘋果位置
        cout << apple->GetIcon();
        for(auto temp : snakes){
            SetCursorPosition(temp->GetX(), temp->GetY());
            cout << temp->GetIcon();
        }
        int tempx, tempy;
        Move(*snakes[0], snakes[0]->GetFace(), tempx, tempy); //測試用
        snakes.push_front(new Snake(tempx, tempy));
        snakes.pop_back();
    }
};

int main(){
    srand(time(NULL));
    Map* map = new Map(20, 20);
    if(kbhit()){
        char ch = getch();
        if(ch == '?'){
            char temp = getch();
            map->Control(temp);
        }
    }
    map->Print();
    Sleep(1000);
    cin.get();
    return 0;
}