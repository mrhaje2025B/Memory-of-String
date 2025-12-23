using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [Header("기본 이동")]
    public float movespeed = 12f;
    public float accel_rate = 6f;
    public float stop_rate = 12f;
    public float jump_pow = 11f;
    public float gravity = 2.5f;

    [Header("스킬 이동")]
    public float dash_length = 5f;
    public float dash_speed = 60f;
    public float dash_cooldown = 0.8f;
    public float spin_pow = 7f;
    public float spin_cooldown = 0.6f;
    public float ankor_speed = 60f;
    public float rope_length;
    public float max_bullet_time;
    public float rope_move_speed = 60f;
    public float bomb_cooddown = 2.5f;
    public float bomb_speed_x = 32f;
    public float bomb_speed_y = 12f;

    [Header("공격, 체력")]
    public int max_hair = 8;
    public int barrier_guage;


}
