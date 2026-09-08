#include <math.h>

function bool32 initialize(Shared_data &shared_data)
{
    State &state = shared_data.state;

    state.x = -25.0f;
    state.y = -50.0f;
    state.z = 0.0f;
    return true;
}

function void update(Shared_data &shared_data)
{
    Keyboard_state &keyboard = shared_data.keyboard;
    State &state = shared_data.state;

    state.x += 1.0f * keyboard.key_d - 1.0f * keyboard.key_a;
    state.y += 1.0f * keyboard.key_w - 1.0f * keyboard.key_s;
    state.z += 1.0f * keyboard.key_q - 1.0f * keyboard.key_e;
}

function void render(Shared_data &shared_data)
{
    State &state = shared_data.state;

    glViewport(0, 0, WINDOW_CLIENT_WIDTH, WINDOW_CLIENT_HEIGHT);
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();

    glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

    glRotatef(state.x, 0.0f, -1.0f, 0.0f);
    glRotatef(-state.y, -1.0f, 0.0f, 0.0f);
    glRotatef(state.z, 0.0f, 0.0f, 1.0f);
    glEnable(GL_BLEND);
    glEnable(GL_DEPTH_TEST);
    glBlendFunc(GL_ONE, GL_ONE_MINUS_SRC_ALPHA);

    float32 head_width = (float32)state.mallet_parameters.heah_width / WINDOW_CLIENT_WIDTH * 2;
    float32 head_height = (float32)state.mallet_parameters.heah_height / WINDOW_CLIENT_WIDTH * 2;
    float32 head_length = (float32)state.mallet_parameters.heah_length / WINDOW_CLIENT_WIDTH * 2;
    int32 head_type = (int32)state.mallet_parameters.head_type;
    float32 head_radius = (float32)state.mallet_parameters.head_diameter / 2 / WINDOW_CLIENT_WIDTH * 2;

    vec3 p0 = {-head_width / 2.0f, -head_length / 2.0f, head_height / 2.0f};
    vec3 p1 = {head_width / 2.0f, -head_length / 2.0f, head_height / 2.0f};
    vec3 p2 = {head_width / 2.0f, head_length / 2.0f, head_height / 2.0f};
    vec3 p3 = {-head_width / 2.0f, head_length / 2.0f, head_height / 2.0f};
    vec3 p4 = {-head_width / 2.0f, -head_length / 2.0f, -head_height / 2.0f};
    vec3 p5 = {head_width / 2.0f, -head_length / 2.0f, -head_height / 2.0f};
    vec3 p6 = {head_width / 2.0f, head_length / 2.0f, -head_height / 2.0f};
    vec3 p7 = {-head_width / 2.0f, head_length / 2.0f, -head_height / 2.0f};

    if(head_type == 0)
    {
        glBegin(GL_QUADS);

        glColor4f(0.0f, 1.0f, 0.0f, 1.25f);
        glVertex3f(p4.x, p4.y, p4.z);
        glVertex3f(p5.x, p5.y, p5.z);
        glVertex3f(p6.x, p6.y, p6.z);
        glVertex3f(p7.x, p7.y, p7.z);

        glColor4f(1.0f, 0.0f, 0.0f, 1.25f);
        glVertex3f(p0.x, p0.y, p0.z);
        glVertex3f(p1.x, p1.y, p1.z);
        glVertex3f(p2.x, p2.y, p2.z);
        glVertex3f(p3.x, p3.y, p3.z);

        glColor4f(0.0f, 1.0f, 1.0f, 1.25f);
        glVertex3f(p0.x, p0.y, p0.z);
        glVertex3f(p4.x, p4.y, p4.z);
        glVertex3f(p7.x, p7.y, p7.z);
        glVertex3f(p3.x, p3.y, p3.z);

        glColor4f(1.0f, 1.0f, 0.0f, 1.25f);
        glVertex3f(p1.x, p1.y, p1.z);
        glVertex3f(p2.x, p2.y, p2.z);
        glVertex3f(p6.x, p6.y, p6.z);
        glVertex3f(p5.x, p5.y, p5.z);

        glColor4f(0.0f, 0.0f, 1.0f, 1.25f);
        glVertex3f(p3.x, p3.y, p3.z);
        glVertex3f(p2.x, p2.y, p2.z);
        glVertex3f(p6.x, p6.y, p6.z);
        glVertex3f(p7.x, p7.y, p7.z);

        glColor4f(1.0f, 0.0f, 1.0f, 1.25f);
        glVertex3f(p0.x, p0.y, p0.z);
        glVertex3f(p1.x, p1.y, p1.z);
        glVertex3f(p5.x, p5.y, p5.z);
        glVertex3f(p4.x, p4.y, p4.z);

        glEnd();
    }
    else
    {
        glBegin(GL_TRIANGLE_FAN);
        glColor4f(0.5f, 1.0f, 0.5f, 1.0f);

        vec3 p8 = {0, head_length / 2.0f, -0.05f};

        glVertex3f(p8.x, p8.y, p8.z);
        for (float32 pi = -3.14f; pi < 3.14; pi += 0.01f)
        {
            vec3 p = {head_radius * sinf(pi), head_length / 2.0f, head_radius * cosf(pi) + p8.z};
            glVertex3f(p.x, p.y, p.z);
        }
        glEnd();

        glBegin(GL_TRIANGLE_FAN);
        glColor4f(1.0f, 0.5f, 0.5f, 1.0f);

        vec3 p9 = {0, -head_length / 2.0f, -0.05f};

        glVertex3f(p9.x, p9.y, p9.z);
        for (float32 pi = -3.14f; pi < 3.14; pi += 0.01f)
        {
            vec3 p = {head_radius * sinf(pi), -head_length / 2.0f, head_radius * cosf(pi) + p9.z};
            glVertex3f(p.x, p.y, p.z);
        }
        glEnd();

        glBegin(GL_TRIANGLE_STRIP);

        for (float32 pi = -3.14f; pi < 3.14; pi += 0.01f)
        {
            vec3 p11 = {head_radius * sinf(pi) + p8.x, p8.y, head_radius * cosf(pi) + p8.z};
            vec3 p22 = {head_radius * sinf(pi) + p9.x, p9.y, head_radius * cosf(pi) + p9.z};
            glColor4f(1.0f / -pi, 1.0f / pi, pi < 0 ? -pi : pi, 1.0f);
            glVertex3f(p11.x, p11.y, p11.z);
            glVertex3f(p22.x, p22.y, p22.z);
        }
        glEnd();
    }

    float32 handle_height = (float32)state.mallet_parameters.handle_height / WINDOW_CLIENT_WIDTH * 2;
    float32 handle_radius = (float32)state.mallet_parameters.handle_diameter / 2 / WINDOW_CLIENT_WIDTH * 2;

    glBegin(GL_TRIANGLE_FAN);
    glColor4f(1.0f, 1.0f, 1.0f, 1.0f);

    vec3 p8 = {0, 0, -head_height / 2.0f};

    glVertex3f(p8.x, p8.y, p8.z);
    for (float32 pi = -3.14f; pi < 3.14; pi += 0.01f)
    {
        vec3 p = {handle_radius * sinf(pi) + p8.x, handle_radius * cosf(pi) + p8.y, p8.z};
        glVertex3f(p.x, p.y, p.z);
    }
    glEnd();

    vec3 p9 = {0, 0, -head_height / 2.0f - handle_height};

    glBegin(GL_TRIANGLE_FAN);
    glColor4f(1.0f, 1.0f, 1.0f, 1.0f);
    glVertex3f(p9.x, p9.y, p9.z);
    for (float32 pi = -3.14f; pi < 3.14; pi += 0.01f)
    {
        vec3 p = {handle_radius * sinf(pi) + p9.x, handle_radius * cosf(pi) + p9.y, p9.z};
        glVertex3f(p.x, p.y, p.z);
    }
    glEnd();

    glBegin(GL_TRIANGLE_STRIP);

    for (float32 pi = -3.14f; pi < 3.14; pi += 0.01f)
    {
        vec3 p11 = {handle_radius * sinf(pi) + p8.x, handle_radius * cosf(pi) + p8.y, p8.z};
        vec3 p22 = {handle_radius * sinf(pi) + p9.x, handle_radius * cosf(pi) + p9.y, p9.z};
        glColor4f(1.0f / -pi, 1.0f / pi, pi < 0 ? -pi : pi, 1.0f);
        glVertex3f(p11.x, p11.y, p11.z);
        glVertex3f(p22.x, p22.y, p22.z);
    }
    glEnd();
}
