#include <bits/stdc++.h>
#include <windows.h>
#include <conio.h>
#include <chrono>
using namespace std;
using namespace chrono;

class CmdHandler{
private:
    COORD coord;
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
public:
    void SetCursorPosition(int x, int y) {
        coord.X = x;
        coord.Y = y;
        SetConsoleCursorPosition(hConsole, coord);
    }
    void HideCursor() {
        CONSOLE_CURSOR_INFO cursorInfo;
        GetConsoleCursorInfo(hConsole, &cursorInfo);
        cursorInfo.bVisible = false;
        SetConsoleCursorInfo(hConsole, &cursorInfo);
    }
};

class Coordinate{
private:
    int x;
    int y;
public:
    Coordinate() : x(0), y(0) {
    }
    Coordinate(int x, int y) : x(x), y(y) {
    }

    int GetX(){ return this->x; };
    int GetY(){ return this->y; };
    void SetX(int x){ this->x = x; };
    void SetY(int y){ this->y = y; };
};

class Role{
protected:
    string name;
    char icon;
    Coordinate* coor;
    int id;
public:
    Role() : Role("role", 'R', 0, 0, 0) {
    }
    Role(string n, char i, int x, int y, int d) : name(n), icon(i), coor(new Coordinate(x, y)), id(d) {
    }
    Role(string n, char i, Coordinate* c, int d) : name(n), icon(i), coor(c), id(d) {
    }
    ~Role(){
        delete coor;
    }
    Coordinate* GetCoor(){ return coor; };
    void SetCoor(Coordinate* c){ this->coor = c; }
    int GetId(){ return this->id; };

    string GetName(){ return name; };
    char GetIcon(){ return icon; };
};

class Snake : public Role{
private:
    char face;
    int rate;
public:
    Snake() : Snake(0, 0, 'L', 500) {
    }
    Snake(int x, int y, char f, int r) : Role("snake", 'S', x, y, 1), rate(r) {
        face = f;
    }
    Snake(Coordinate* c, char f, int r):Role("snake", 'S', c, 1), rate(r) {
        face = f;
    }

    char GetFace(){ return face; };
    void SetFace(char f){ this->face = f; };
    int GetRate(){ return rate; };
    void SetRate(int r){ this->rate = r; };
};

class Apple : public Role{
public:
    Apple() : Role("apple", 'A', 0, 0, 2) {
    }
    Apple(int x, int y) : Role("apple", 'A', x, y, 2) {
    }
    Apple(Coordinate* c) : Role("apple", 'A', c, 2) {
    }
};

class Map{
private:
    int width;
    int height;
    int gameover = 0;
    int status = 0;
    CmdHandler cmdh; 
    vector<vector<int>> v;

    void PrintFrame(){ 
        for(int i = 0; i < width + 2; i++){
            cmdh.SetCursorPosition(i, 0);
            cout << '#';
        }
        for(int i = 0; i < height + 2; i++){
            cmdh.SetCursorPosition(0, i);
            cout << '#';
            cmdh.SetCursorPosition(width + 1, i);
            cout << '#';
        }
        cmdh.SetCursorPosition(0, height + 1);
        for(int i = 0; i < width + 2; i++){
            cout << '#';
        }

    }
public:
    Map() : Map(20, 20){
    }
    Map(int w, int h) : width(w), height(h) {
        cmdh.HideCursor();
        v = vector<vector<int> >(height, vector<int>(width, 0));
        PrintFrame();
    }

    ~Map(){
    }

    Coordinate* GetSpace(){
        int x = rand() % width;
        int y = rand() % height;
        while(v[y][x] != 0){
            x = rand() % width;
            y = rand() % height;
        }
        return new Coordinate(x, y);
    }

    int CheckSpace(Coordinate* c){
        if(c == nullptr) return -1;
        if(c->GetX() < 0 || c->GetX() >= width || c->GetY() < 0 || c->GetY() >= height) return -1;
        return v[c->GetY()][c->GetX()];
    }

    int CheckSpace(int& x, int& y){
        if(x < 0 || x >= width || y < 0 || y >= height) return -1;
        return v[y][x];
    }

    void Mark(Coordinate* c, int id){
        v[c->GetY()][c->GetX()] = id;
    }

    void Mark(int& x, int& y, int id){
        v[y][x] = id;
    }

    void Unmark(Coordinate* c){
        v[c->GetY()][c->GetX()] = 0;
    }

    void Unmark(int& x, int& y){
        v[y][x] = 0;
    }

    int GetGameOver(){
        return gameover;
    };
    void SetGameOver(int g){
        gameover = g;
    };
    int GetWidth(){ return width; };
    int GetHeight(){ return height; };
};

class Snakes{
private:
    CmdHandler cmdh;
    deque<Snake*> snakes;
    Map* map1;
    int status = 0;
    const int id = 1;
public:
    Snakes(Map* m) : map1(m){
        Coordinate* c = map1->GetSpace();
        int x = c->GetX();
        int y = c->GetY();
        for(int i = 0; i < 3; i++){
            Coordinate* part = new Coordinate(x + i, y);
            snakes.push_back(new Snake(part->GetX(), part->GetY(), 'L', 500));
            map1->Mark(part, id);
        }
    }
    ~Snakes(){
        for(auto temp : snakes){
            delete temp;
        }
    }

    void Turn(char keycode){
        switch(keycode){
            case 72:
                snakes.front()->SetFace('U');
                break;
            case 80: 
                snakes.front()->SetFace('D');
                break;
            case 75: 
                snakes.front()->SetFace('L');
                break;
            case 77: 
                snakes.front()->SetFace('R');
                break;
            default:
                break;
        }
    }

    void Hasten(int command){
        int rate = snakes.front()->GetRate();
        if(command == 43){ 
            if(rate >= 400) rate -= 50; 
        } else if(command == 45){ 
            if(rate <= 600) rate += 50; 
        }
        snakes.front()->SetRate(rate); 
    }

     int Move(){
        int status = 0;
        int height = map1->GetHeight();
        int width = map1->GetWidth();
        int x = snakes[0]->GetCoor()->GetX();
        int y = snakes[0]->GetCoor()->GetY();
        switch(snakes[0]->GetFace()){
            case 'U':
                if(y - 1 >= 0) y -= 1;
                else status = 1;
                break;
            case 'D':
                if(y + 1 < height) y += 1;
                else status = 1;
                break;
            case 'L':
                if(x - 1 >= 0) x -= 1;
                else status = 1;
                break;
            case 'R':
                if(x + 1 < width) x += 1;
                else status = 1; 
                break;
        }
        if(status != 0) return status;
        if(map1->CheckSpace(x, y) == 1){
            status = 2;
            return status;
        }
        if(map1->CheckSpace(x, y) == 2){
            status = 3;
        }
        snakes.push_front(new Snake(x, y, snakes.front()->GetFace(), snakes.front()->GetRate()));
        map1->Mark(snakes.front()->GetCoor(), snakes.front()->GetId());
        if(status != 3){
            map1->Unmark(snakes.back()->GetCoor());
            delete snakes.back();
            snakes.pop_back();
        }
        return status;
    }

    void Print(){
        cmdh.SetCursorPosition(snakes.front()->GetCoor()->GetX() + 1, snakes.front()->GetCoor()->GetY() + 1);
        cout << snakes.front()->GetIcon();
        cmdh.SetCursorPosition(snakes.back()->GetCoor()->GetX() + 1, snakes.back()->GetCoor()->GetY() + 1);
        cout << ' ';
    }
    deque<Snake*>& GetSnakes(){ return snakes; }
    int GetStatus(){ return status; }
    int GetRate(){ return snakes.front()->GetRate(); }
};

class Apples{
private:
    vector<Apple*> apples;
    Map* map1;
    const int id = 2;
    CmdHandler cmdh;
public:
    Apples(Map* m, int num) : map1(m){
        for(int i = 0; i < num; i++) AddApple();
    }
    ~Apples(){
        for(auto apple : apples){
            delete apple;
        }
    }

    void AddApple(){
        Coordinate* c = map1->GetSpace();
        apples.push_back(new Apple(c));
        map1->Mark(c, id);
    }

    void GotHit(Coordinate* c){
        auto tar = find_if(apples.begin(), apples.end(), [c](auto x){ return x->GetCoor()->GetX() == c->GetX() && x->GetCoor()->GetY() == c->GetY(); });
        if(tar != apples.end()){
            map1->Unmark((*tar)->GetCoor());
            delete *tar;
            apples.erase(tar);
            AddApple();
        }
    }

    void Print(){
        for(auto apple : apples){
            cmdh.SetCursorPosition(apple->GetCoor()->GetX() + 1, apple->GetCoor()->GetY() + 1);
            cout << apple->GetIcon();
        }
    }
    vector<Apple*>& GetApples(){ return apples; }
};

class SnakeGame{
private:
    int score = 0;
    int pace = 3;
    int level = 1;
    Map* map1;
    Snakes* snakes;
    Apples* apples;
    CmdHandler cmdh;
    steady_clock::time_point pstart = steady_clock::now();
    steady_clock::time_point Snstart = steady_clock::now();
public:
    SnakeGame() : map1(new Map(20, 20)) {
        srand(time(NULL));
        snakes = new Snakes(map1);
        apples = new Apples(map1, 3);
    }
    ~SnakeGame(){
        delete map1;
        delete snakes;
        delete apples;
    }

    void Control(){
        int ch = getch();
        if (ch == 224 || ch == 0) {
            char arrow = getch();   
            snakes->Turn(arrow);   
            return;
        }
        if(ch == 43 || ch == 45){
            snakes->Hasten(ch);
            if(ch == 43) {
                if(pace < 5) pace++;
            } else if(ch == 45) {
                if(pace > 1) pace--;
            }
            return;
        }
    }

    void Print(){
        cmdh.SetCursorPosition(25, 23);
        cout << "Time:" << duration_cast<seconds>(steady_clock::now() - pstart).count() << "s";
        cmdh.SetCursorPosition(25, 14);
        cout << "Score: " << score;
        cmdh.SetCursorPosition(25, 15);
        cout << "Pace: " << pace;
        cmdh.SetCursorPosition(25, 16);
        cout << "Level: " << level;
    }

    void Snake(){
        while(true){
            if(map1->GetGameOver() == 1){ 
                system("cls");
                cout << "Game Over" << endl;
                break;
            }
            if(kbhit()) {
                Control();
            }
            if(level < 5){
                level = (score / 10) + 1;
            }
            steady_clock::time_point now = steady_clock::now();
            if(duration_cast<milliseconds>(steady_clock::now() - Snstart).count() >= (snakes->GetRate() - (25 * (level - 1)))){
                Coordinate* c = new Coordinate();
                int result = snakes->Move();
                snakes->Print(); //印出蛇
                if(result == 3){
                    apples->GotHit(snakes->GetSnakes().front()->GetCoor());
                    score++;
                }
                if(result == 1 || result == 2){ 
                    map1->SetGameOver(1); 
                }
                Snstart = steady_clock::now(); 
                apples->Print();
                Print();
            }
        }
    }
};

int main(){
    SnakeGame* game = new SnakeGame();
    game->Snake();
    delete game;
    cin.get();
    return 0;
}