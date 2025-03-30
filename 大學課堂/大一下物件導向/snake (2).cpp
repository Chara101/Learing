#include <bits/stdc++.h>
#include <windows.h>
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
public:
    Snake() : Role("snake", 'S', 0, 0) {
    }
    Snake(string n, char i, int x, int y) : Role(n, i, x, y) {
    }
};

class Apple : public Role{ // 蘋果
public:
    Apple() : Role("apple", 'S', 0, 0) {
    }
    Apple(int x, int y) : Role("apple", 'S', x, y) {
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
    vector<Role*> snakes;
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
public:
    Map(){
        width = 20;
        height = 20;
        Map(width, height);
        // v = new vector<vector<int>>(height, vector<int>(width, 0));
        // apple = SetPos(new Apple(0, 0));
    }
    Map(int w, int h) : width(w), height(h) {
        v = vector<vector<int> >(height, vector<int>(width, 0));
        apple = new Apple();
        SetPos(*apple);
        snakes.push_back(new Snake("snake", 'S', 0, 0));
        SetPos(*snakes[0]);
        HideCursor();
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

    void Control(){

    }

    void Print(){
        system("cls");
        SetCursorPosition(apple->GetX(), apple->GetY());
        cout << apple->GetIcon();
    }
};

int main(){
    srand(time(NULL));
    Map* map = new Map(20, 20);
    map->Print();
    cin.get();
    return 0;
}