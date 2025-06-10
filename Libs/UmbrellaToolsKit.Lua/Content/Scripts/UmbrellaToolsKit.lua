Vector3 = {}
Vector3.__index = Vector3

function Vector3.new(x, y, z)
    local self = setmetatable({}, Vector3)
    self.x = x or 0.0
    self.y = y or 0.0
    self.z = z or 0.0
    return self
end

function Vector3.__add(a, b)
    return Vector3.new(a.x + b.x, a.y + b.y, a.z + b.z)
end

function Vector3.__sub(a, b)
    return Vector3.new(a.x - b.x, a.y - b.y, a.z - b.z)
end

function Vector3.__mul(a, b)
    if type(a) == "number" then
        return Vector3.new(a * b.x, a * b.y, a * b.z)
    elseif type(b) == "number" then
        return Vector3.new(a.x * b, a.y * b, a.z * b)
    else
        return a.x * b.x + a.y * b.y + a.z * b.z
    end
end

function Vector3.__unm(a)
    return Vector3.new(-a.x, -a.y, -a.z)
end

function Vector3.__eq(a, b)
    return a.x == b.x and a.y == b.y and a.z == b.z
end

function Vector3:mag()
    return math.sqrt(self.x^2 + self.y^2 + self.z^2)
end

function Vector3:normalize()
    local m = self:mag()
    if m == 0 then
        return Vector3.new(0, 0, 0)
    end
    return Vector3.new(self.x / m, self.y / m, self.z / m)
end

function Vector3.__tostring(v)
    return string.format("Vector3(%.2f, %.2f, %.2f)", v.x, v.y, v.z)
end

-- Vector2

Vector2 = {}
Vector2.__index = Vector2

function Vector2.new(x, y)
    local self = setmetatable({}, Vector2)
    self.x = x or 0.0
    self.y = y or 0.0
    return self
end

function Vector2.__add(a, b)
    return Vector2.new(a.x + b.x, a.y + b.y)
end

function Vector2.__sub(a, b)
    return Vector2.new(a.x - b.x, a.y - b.y)
end

function Vector2.__mul(a, b)
    if type(a) == "number" then
        return Vector2.new(a * b.x, a * b.y)
    elseif type(b) == "number" then
        return Vector2.new(a.x * b, a.y * b)
    else
        return a.x * b.x + a.y * b.y
    end
end

function Vector2.__unm(a)
    return Vector2.new(-a.x, -a.y)
end

function Vector2.__eq(a, b)
    return a.x == b.x and a.y == b.y
end

function Vector2:mag()
    return math.sqrt(self.x^2 + self.y^2)
end

function Vector2:normalize()
    local m = self:mag()
    if m == 0 then
        return Vector2.new(0, 0)
    end
    return Vector2.new(self.x / m, self.y / m)
end

function Vector2.__tostring(v)
    return string.format("Vector2(%.2f, %.2f)", v.x, v.y)
end

------------------------------------------------------------
return {
    Vector2 = Vector2,
    Vector3 = Vector3
}