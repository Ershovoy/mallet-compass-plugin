#define VC_EXTRALEAN
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
typedef double DOUBLE;
#include <gl/GL.h>

#include "windows_main.h"

#include "application.cpp"

File ReadFile(char16* file_name)
{
    File result = {};

    HANDLE fileHandle = CreateFileW(file_name, GENERIC_READ, FILE_SHARE_READ, 0, OPEN_EXISTING, 0, 0);
    if (fileHandle == INVALID_HANDLE_VALUE)
        return result;

    DWORD fileSize = GetFileSize(fileHandle, NULL);
    if (fileSize != INVALID_FILE_SIZE)
    {
        result.memory = VirtualAlloc(0, fileSize, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
        if (result.memory)
        {
            if (!(ReadFile(fileHandle, result.memory, fileSize, (LPDWORD)&result.size, 0) &&
                  ((uint32)result.size == fileSize)))
            {
                VirtualFree(result.memory, 0, MEM_RELEASE);
                result = {};
            }
        }
    }

    CloseHandle(fileHandle);
    return result;
}

FILETIME GetLastFileWriteTime(WCHAR* fileName)
{
    FILETIME lastFileWriteTime = {};

    WIN32_FILE_ATTRIBUTE_DATA fileAttributeData;
    if(GetFileAttributesExW(fileName, GetFileExInfoStandard, &fileAttributeData))
    {
        lastFileWriteTime = fileAttributeData.ftLastWriteTime;
    }

    return lastFileWriteTime;
}

LONG64 GetCounter()
{
    LARGE_INTEGER counter;
    QueryPerformanceCounter(&counter);
    return counter.QuadPart;
}

LONG64 GetCounterFrequency()
{
    LARGE_INTEGER counterFrequency;
    QueryPerformanceFrequency(&counterFrequency);
    return counterFrequency.QuadPart;
}

BOOL InitializeOpenGL(HDC deviceContext)
{
    PIXELFORMATDESCRIPTOR desiredPixelFormat = {};
    desiredPixelFormat.nSize = sizeof(PIXELFORMATDESCRIPTOR);
    desiredPixelFormat.nVersion = 1;
    desiredPixelFormat.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER;
    desiredPixelFormat.iPixelType = PFD_TYPE_RGBA;
    desiredPixelFormat.cColorBits = 32;
    desiredPixelFormat.cAlphaBits = 8;

    int suggestedPixelFormatIndex = ChoosePixelFormat(deviceContext, &desiredPixelFormat);
    PIXELFORMATDESCRIPTOR suggestedPixelFormat;
    if (!DescribePixelFormat(deviceContext, suggestedPixelFormatIndex, sizeof(PIXELFORMATDESCRIPTOR), &suggestedPixelFormat))
        return FALSE;

    if (!SetPixelFormat(deviceContext, suggestedPixelFormatIndex, &suggestedPixelFormat))
        return FALSE;

    HGLRC openGlRenderingContext = wglCreateContext(deviceContext);
    wglMakeCurrent(deviceContext, openGlRenderingContext);

    return TRUE;
}

LRESULT CALLBACK WindowProcedure(_In_ HWND   window,
                                 _In_ UINT   message,
                                 _In_ WPARAM wParam,
                                 _In_ LPARAM lParam)
{
    LRESULT result = {};
    switch(message)
    {
        case WM_DESTROY:
        {
            GlobalIsRunning = false;
            PostQuitMessage(0);
            break;
        }
        case WM_KEYDOWN:
        case WM_KEYUP:
        case WM_SYSKEYDOWN:
        case WM_SYSKEYUP:
        {
            WORD vkCode = LOWORD(wParam);
            WORD keyFlags = HIWORD(lParam);

            BOOL isKeyDown = (keyFlags & KF_UP) != KF_UP;
            BOOL wasKeyDown = (keyFlags & KF_REPEAT) == KF_REPEAT;

            if (isKeyDown != wasKeyDown)
            {
                switch (vkCode)
                {
                    case 'W':
                    case VK_UP:
                    {
                        global_shared_data.keyboard.key_w = isKeyDown;
                        break;
                    }
                    case 'A':
                    case VK_LEFT:
                    {
                        global_shared_data.keyboard.key_a = isKeyDown;
                        break;
                    }
                    case 'S':
                    case VK_DOWN:
                    {
                        global_shared_data.keyboard.key_s = isKeyDown;
                        break;
                    }
                    case 'D':
                    case VK_RIGHT:
                    {
                        global_shared_data.keyboard.key_d = isKeyDown;
                        break;
                    }
                    case 'Q':
                    {
                        global_shared_data.keyboard.key_q = isKeyDown;
                        break;
                    }
                    case 'E':
                    {
                        global_shared_data.keyboard.key_e = isKeyDown;
                        break;
                    }
                }
            }
            break;
        }
        case WM_KILLFOCUS:
        {
            global_shared_data.keyboard = {};
            break;
        }
        default:
        {
            result = DefWindowProcW(window, message, wParam, lParam);
            break;
        }
    }
    return result;
}

HWND InitializeWindow(HINSTANCE instance)
{
    WNDCLASSEXW windowClass = {};
    windowClass.cbSize        = sizeof(WNDCLASSEXW);
    windowClass.style         = CS_HREDRAW | CS_VREDRAW | CS_OWNDC;
    windowClass.lpfnWndProc   = WindowProcedure;
    windowClass.hInstance     = instance;
    windowClass.hIcon         = NULL;
    windowClass.hIconSm       = windowClass.hIcon;
    windowClass.hCursor       = LoadCursorW(NULL, MAKEINTRESOURCEW(32512));
    windowClass.lpszClassName = L"Mallet by OpenGL";

    RegisterClassExW(&windowClass);

    DWORD windowExtendedStyle = WS_EX_OVERLAPPEDWINDOW;
    DWORD windowStyle = WS_OVERLAPPEDWINDOW | WS_VISIBLE;

    RECT clientRectangle = { 0, 0, WINDOW_CLIENT_WIDTH, WINDOW_CLIENT_HEIGHT };
    RECT windowRectangle = clientRectangle;
    AdjustWindowRectEx(&windowRectangle, windowStyle, FALSE, windowExtendedStyle);
    LONG requiredWindowWidth = windowRectangle.right - windowRectangle.left;
    LONG requiredWindowHeight = windowRectangle.bottom - windowRectangle.top;

    HWND window = CreateWindowExW(windowExtendedStyle, windowClass.lpszClassName,
                                  windowClass.lpszClassName, windowStyle,
                                  CW_USEDEFAULT, CW_USEDEFAULT, requiredWindowWidth, requiredWindowHeight,
                                  NULL, NULL, instance, NULL);

    return window;
}

int WINAPI wWinMain(_In_     HINSTANCE instance,
                    _In_opt_ HINSTANCE previousInstance,
                    _In_     LPWSTR    lpCmdLine,
                    _In_     int       nShowCmd)
{
    FILETIME previousFileWriteTime = GetLastFileWriteTime(GlobalMalletParametersFileName);

    File file = ReadFile(GlobalMalletParametersFileName);
    global_shared_data.state.mallet_parameters = *(Mallet_parameters*)file.memory;

    HWND window = InitializeWindow(instance);
    if (window)
    {
        HDC deviceContext = GetDC(window);

        if (!InitializeOpenGL(deviceContext))
            return -1;

        LONG64 perfomanceCounterFrequency = GetCounterFrequency();
        DOUBLE secondsPerUpdate = 1.0 / 60.0;

        LONG64 previousCounter = GetCounter();
        DOUBLE accumulator = 0.0;

        GlobalIsRunning = initialize(global_shared_data);
        while (GlobalIsRunning)
        {
            FILETIME currentFileWriteTime = GetLastFileWriteTime(GlobalMalletParametersFileName);
            if (CompareFileTime(&previousFileWriteTime, &currentFileWriteTime) != 0)
            {
                file = ReadFile(GlobalMalletParametersFileName);
                if(file.memory)
                {
                    global_shared_data.state.mallet_parameters = *(Mallet_parameters*)file.memory;
                }
            }

            MSG message = {};
            while (PeekMessageW(&message, NULL, 0, 0, PM_REMOVE))
            {
                TranslateMessage(&message);
                DispatchMessageW(&message);
            }

            LONG64 currentCounter = GetCounter();
            LONG64 counterElapsed = currentCounter - previousCounter;
            DOUBLE secondsElapsed = (DOUBLE)counterElapsed / perfomanceCounterFrequency;
            accumulator += secondsElapsed;
            previousCounter = currentCounter;
            while (accumulator >= secondsPerUpdate)
            {
                update(global_shared_data);

                accumulator -= secondsPerUpdate;
                if (accumulator < secondsPerUpdate)
                {
                    render(global_shared_data);
                    SwapBuffers(deviceContext);
                }
            }

            Sleep(1);
        }
    }
    return 0;
}
