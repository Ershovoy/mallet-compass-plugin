#include "basic_types.h"

struct File
{
    void* memory;
    int32 size;
};

struct Mallet_parameters
{
    float64 heah_width;
    float64 heah_height;
    float64 heah_length;
    float64 handle_height;
    float64 handle_diameter;
    float64 head_diameter;
    int32 head_type;
};

struct Keyboard_state
{
    bool32 key_w;
    bool32 key_a;
    bool32 key_s;
    bool32 key_d;
    bool32 key_q;
    bool32 key_e;
};

struct State
{
    float32 x;
    float32 y;
    float32 z;

    Mallet_parameters mallet_parameters;
};

struct Shared_data
{
    Keyboard_state   keyboard;
    State            state;
};
