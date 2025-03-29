#include <bits/stdc++.h>
using namespace std;

class Role{
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

class Snake : public Role{
public:
    Snake() : Role("snake", 'S', 0, 0) {
    }
    Snake(string n, char i, int x, int y) : Role(n, i, x, y) {
    }
};

class Apple : public Role{
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
private:
    int width;
    int height;
    Apple* apple;
    vector<Role*> snakes;
    vector<vector<int>> v;
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
        
    }
};

int main(){
    srand(time(NULL));
    return 0;
}