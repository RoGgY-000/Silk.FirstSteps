#version 400  core

in vec3 fNormal;
in vec3 fPos;

struct Light {
    vec3 position;
    vec3 color;
    float ambient;
    float diffuse;
    float specular;
    float constant;
    float linear;
    float quadratic;
};

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

const int MAX_LIGHTS = 16;

uniform int lightCount;
uniform Light lights[MAX_LIGHTS];
uniform Material material;
uniform vec3 viewPos;

out vec4 FragColor;

vec3 calcLight (Light light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDirection = normalize(light.position - fragPos);
    vec3 halfwayDir = normalize(lightDirection + viewDir);
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / ( light.constant + light.linear * distance + light.quadratic * pow(distance, 2));

    vec3 ambient = light.ambient * material.ambient;

    float diff = max(dot(normal, lightDirection), 0.0);
    vec3 diffuse = light.diffuse * (diff * material.diffuse);

    float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * material.specular;

    return light.color * (ambient + diffuse + specular) * attenuation;
}

void main()
{
    vec3 norm = normalize(fNormal);
    vec3 viewDirection = normalize(viewPos - fPos);
    vec3 result = vec3(0.0);
    for (int i = 0; i < lightCount; i++)
    {
        result += calcLight(lights[i], norm, fPos, viewDirection);
    }
    FragColor = vec4(result, 1.0);
}