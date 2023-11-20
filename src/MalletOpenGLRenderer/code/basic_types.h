#include <stdint.h>

#define function static
#define global   static
#define local    static

typedef uint32_t bool32;

typedef int8_t  int8;
typedef int16_t int16;
typedef int32_t int32;
typedef int64_t int64;

typedef uint8_t  uint8;
typedef uint16_t uint16;
typedef uint32_t uint32;
typedef uint64_t uint64;

typedef float  float32;
typedef double float64;

typedef char    char8;
typedef wchar_t char16;

union vec3
{
    struct
    {
        float32 x;
        float32 y;
        float32 z;
    };
    struct
    {
        float32 width;
        float32 height;
        float32 length;
    };

    const float32 operator[](int32 index) const
    {
        return ((float32*)this)[index];
    }
    float32& operator[](int32 index)
    {
        return ((float32*)this)[index];
    }
};
