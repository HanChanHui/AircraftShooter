using Consts;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


public class Shooter : MonoBehaviour
{

    [Header("Basic")]
    public ShootingType shootingType;
    public Transform muzzle;
    

    [Header("Bullet")]
    //public string bulletType;
    public BulletType bulletType;
    public int power;
    public float speed;
    public float speedRate;
    public float angle;
    public float angleRate;
    public float startDistance;
    public float lifeTime;
    //public string BulletType { get { return bulletType; } set { bulletType = value; } }

    [Header("Custom Shape")]
    public float rot = 0f;
    [Range(3, 7)] public int vertex = 3;
    [Range(1, 5)] public float sup = 3;
    public bool rotateBulletCore = false;
    public Vector3 rotateBulletOffset;
    int m;
    float a;
    float phi;
    List<float> cs_v = new List<float>();
    List<float> cs_xx = new List<float>();


    bool isCritical;
    int currPower;
    public int CurrPower { set { this.currPower = value; } get { return this.currPower; } }
    public float BulletSpeed { set { this.speed = value; } get { return this.speed; } }
    public int BulletCount { get { return this.count; } set { this.count = value; } }
    public float BulletAngle { get { return this.angle; } set { this.angle = value; } }
    public float NwayAngle { get { return this.angleRange; } set { this.angleRange = value; } }
    public float AngleRate { get { return angleRate; } set { angleRate = value; } }
    public int Vertex { get { return vertex; } set { vertex = value; } }
    public float Sup { get { return sup; } set { sup = value; } }
    public float Rot { get { return rot; } set { rot = value; } }

    public float LifeTime { get { return lifeTime; } set { lifeTime = value; } }

    int prevPower;
    List<Bullet> bulletList = new();

    [Header("Forward")]
    public float forwardAngleSpeed;


    [Header("Nway")]
    public float angleRange;
    public int count;

    [Header("Multiple")]
    public float multipleStep;
    public float multipleRange;

    [Header("Homing")]
    public float turnSpeed;
    public bool decreaseHomingSpeed;
    public float homingSpeedRate;
    public float delayTime;

    [Header("Rolling N-way")]
    public int nWayCount;

    [Header("Waving N-way")]
    public float wavingAngleRange;
    public int cycle;

    [Header("Placed")]
    public float moveTime;
    public float stopTime;
    public float placedStopSpeed;

    [Header("Aiming")]
    public Transform targetTransform;
    public bool stopAttackCooltime = true;
    public float targetfixedAngle;

    [Header("Spreading")]
    public float groupSpeed;
    public float groupCount;
    public float speedRange;

    [Header("Overtaking")]
    public float groupAngle;
    public float groupInterval;

    [Header("Arc")]
    public float arrivalTime;
    public float height;

    [Header("Pattern")]
    public int pat_width;
    public int pat_height;
    protected string pattern =
    "                                   \n" +
    "                                   \n" +
    "                                   \n" +
    "####  #   # #     #     ##### #####\n" +
    "#   # #   # #     #     #       #  \n" +
    "####  #   # #     #     ####    #  \n" +
    "#   # #   # #     #     #       #  \n" +
    "####  ##### ##### ##### #####   #  ";

    [Header("CustomShapeForward")]
    public ShapeType shapeType;
    public int vertexShape;
    public float radius;
    public float angleSpeed;
    public int segments;
    public int circleAngle;

    [Header("GoblinFire")]
    public float wavingRange;
    public float wavingCycle;


    [Header("AttackCoolTime")]
    public float attackCooltime = 0f;
    public float attackTime = 0f;
    public float stopAttackDelay = 0f;
    public float attackTimeReset = 0f;
    public float attackFixedTime = 0f;
    public bool canAttack = true;

    delegate void ShootFunc();
    ShootFunc shootFunc;
    bool isInitialized;
    bool isRunning = false;
    public bool IsRunning { get { return isRunning; } set { isRunning = value; } }
    public string jsonName;

    int isCalculatedDamage;
    public int CalculatedDamage { get { return isCalculatedDamage; } set { isCalculatedDamage = value; } }
    public bool CheckOutBound { get; set; }
    bool penetration;
    int penetrationCount;

    public void Init(Transform muzzle = null, Transform target = null) {
        if (isInitialized) {
            return;
        }

        isCalculatedDamage = power;

        if (target) {
            this.targetTransform = target;
        }

        if (muzzle) {
            SetMuzzle(muzzle);
        }

        SetType(this.shootingType);

        isInitialized = true;
        isRunning = true;

        if (shootingType == ShootingType.CustomShape) {
            InitCustomShape();
        }

       
    }

    public void SetMuzzle(Transform muzzle) {
        this.muzzle = muzzle;
    }

    public void SetType(ShootingType type) {
        this.shootingType = type;
        shootFunc = GetShootFunc(type);
    }

    public void RemoveType(ShootingType type)
    {
        shootFunc -= GetShootFunc(type);
    }

    public void ResetDamage() {
        currPower = power;
    }

    public void PowerUp(int rate) {
        prevPower = currPower;
        currPower = (int)(currPower * rate / 100);
    }

    public void EndPowerUP() {
        currPower = prevPower;
    }

    public void LevelupBulletPower(int rate, bool duringPowerup) {
        if (duringPowerup) {
            prevPower = (int)(prevPower * rate / 100);
        } else {
            PowerUp(rate);
        }
    }

    public void StopAttackCooltime(bool _stopAttackCooltime)
    {
        stopAttackCooltime = _stopAttackCooltime;
    }

    public void Shoot() 
    {
        shootFunc();
    }

    public IEnumerator CoCheckDistance() 
    {
        while (isRunning)
        {
            if (canAttack)
            {
                StartCoroutine(CoAttack());
                yield break;
            }
            yield return new WaitForSeconds(0f);
        }
        yield return null;
    }

    private IEnumerator CoAttack()
    {
        if (isRunning)
        {
            yield return new WaitForSeconds(attackTime);
            Shoot();

            StartCoroutine(CoAttackCooltime());
            StartCoroutine(CoCheckDistance());
        }
    }


    private IEnumerator CoAttackCooltime()
    {
        if (isRunning)
        {
            canAttack = false;
            yield return new WaitForSeconds(attackTimeReset);
            canAttack = true;
        }
    }

    public IEnumerator CoStopAttackCooltime()
    {
        if(attackFixedTime > 0)
        {
            StartCoroutine(CoFixedAttackCooltime());
        }

        while(isRunning)
        {
            attackTimeReset = 0f;
            yield return new WaitForSeconds(attackCooltime);
            attackTimeReset = stopAttackDelay;
            yield return new WaitForSeconds(attackTimeReset);
        }
    }

    public IEnumerator CoFixedAttackCooltime()
    {
        yield return new WaitForSeconds(attackFixedTime);
        isRunning = false;
    }

    public void StartShoot()
    {
        Init();
        StopAllCoroutine();
        isRunning = true;
        StartCoroutine(CoCheckDistance());
        StartCoroutine(CoStopAttackCooltime());
    }

    // temp
    public void Shoot(int damage, bool isCritical) 
    {
        isCalculatedDamage = damage;
        this.isCritical = isCritical;

        shootFunc();
    }

    void BasicShoot(float speed, float speedRate, float angle, float angleRate, Vector3 pos) {
        Bullet bullet = HSPoolManager.Instance.NewItem<Bullet>(bulletType.ToString());
        if (bullet) 
        {
            if (penetration) {
                bullet.SetPenetration(true, penetrationCount);
            }

            bullet.CheckOutBound = this.CheckOutBound;
            bullet.Create(pos, muzzle.rotation, isCalculatedDamage, speed,
                    speedRate, angle, angleRate, isCritical, startDistance, lifeTime);

            if (rotateBulletCore) {
                var offset = rotateBulletOffset + new Vector3(0, angle, 0);
                bullet.RotateBulletCore(offset);
            }

        }
    }

    void BasicShoot() 
    {
        BasicShoot(speed, speedRate, angle, angleRate, muzzle.position);
    }

    void ForwardShoot() 
    {
        NwayShoot(speed, speedRate, angle + (forwardAngleSpeed * Time.time % 360), angleRate, angleRange, count);
    }

    void NwayShoot(float speed, float speedRate, float angle, float angleRate, float angleRange, int count) 
    {
        if (count > 1) {
            for (int i = 0; i < count; i++) 
            {
                BasicShoot(speed, speedRate, angle + angleRange * ((float)i / (count - 1) - 0.5f), angleRate, muzzle.position);
            }
        } else {
            BasicShoot(speed, speedRate, angle, angleRate, muzzle.position);
        }
    }

    void NwayShoot() 
    {
        NwayShoot(speed, speedRate, angle, angleRate, angleRange, count);
    }

    void CircleShoot() 
    {
        NwayShoot(speed, speedRate, angle, angleRate, angleRange - angleRange / count, count);
    }

    void RandomNwayShoot() 
    {
        for (int i = 0; i < count; i++) 
        {
            BasicShoot(speed, speedRate, angle + angleRange * (UnityEngine.Random.Range(0, 1.0f) - 0.5f) + (forwardAngleSpeed * Time.time % 360), angleRate, muzzle.position);
        }
    }

    void RandomCircleShoot() 
    {
        for (int i = 0; i < count; i++) {
            BasicShoot(speed, speedRate, UnityEngine.Random.Range(0, 360f), angleRate, muzzle.position);
        }
    }

    void MultipleShoot() {
        if (count > 1) 
        {
            for (int i = 0; i < count; i++) {
                Vector3 pos = muzzle.position;
                pos.x += multipleRange * ((float)i / (count - 1) - 0.5f);
                pos = RotatePointAroundPivot(pos, muzzle.position, muzzle.rotation.eulerAngles);

                BasicShoot(speed, speedRate, angle, angleRate, pos);
            }
        } 
        else 
        {
            BasicShoot();
        }
    }

    void HomingShoot() {
        HomingBullet bullet = HSPoolManager.Instance.NewItem<HomingBullet>(bulletType.ToString());
        if (bullet) {
            bullet.Init(turnSpeed, targetTransform, homingSpeedRate, decreaseHomingSpeed);
            bullet.CheckOutBound = this.CheckOutBound;
            bullet.Create(muzzle.position, muzzle.rotation, isCalculatedDamage,
                    speed, speedRate, angle, angleRate, isCritical, startDistance, lifeTime);
        }
    }

    void DelayHomingShoot() {
        DelayHomingBullet bullet = HSPoolManager.Instance.NewItem<DelayHomingBullet>(bulletType.ToString());
        if (bullet) {
            bullet.Init(turnSpeed, delayTime);
            bullet.Create(muzzle.position, muzzle.rotation, isCalculatedDamage,
                    speed, speedRate, angle, angleRate, isCritical, startDistance, lifeTime);
        }
    }

    void RandomHomingShoot() {
        HomingBullet bullet = HSPoolManager.Instance.NewItem<HomingBullet>(bulletType.ToString());
        if (bullet) 
        {
            bullet.Init(turnSpeed, null, homingSpeedRate, decreaseHomingSpeed);
            bullet.CheckOutBound = this.CheckOutBound;
            bullet.Create(muzzle.position, muzzle.rotation, isCalculatedDamage,
                    speed, speedRate, angle + angleRange * (UnityEngine.Random.Range(0, 1.0f) - 0.5f), angleRate, isCritical, startDistance, lifeTime);
        }
    }

    void RandomDirectionalShoot() 
    {
        BasicShoot(speed, speedRate, angle + angleRange * (UnityEngine.Random.Range(0, 1.0f) - 0.5f), angleRate, muzzle.position);
    }

    void RollingNway() 
    {
        for (int i = 0; i < nWayCount; i++) 
        {
            NwayShoot(speed, speedRate, angle + (float)(i * 360) / nWayCount + (forwardAngleSpeed * Time.time % 360), angleRate, angleRange, count);
        }
    }

    void WavingNwayShoot() 
    {
        NwayShoot(speed, speedRate, angle + wavingAngleRange * Mathf.Sin(Time.time * cycle) + (forwardAngleSpeed * Time.time % 360),
                angleRate, angleRange, count);
        Debug.Log(Mathf.Sin(Time.time * cycle));
    }

    void WavingCircleShoot() 
    {
        NwayShoot(speed, speedRate, angle + wavingAngleRange * Mathf.Sin(Time.time * cycle),
                angleRate, 360f - 360f / count, count);
    }

    void PlacedShoot() 
    {
        Bullet bullet = HSPoolManager.Instance.NewItem<Bullet>(bulletType.ToString());
        if (bullet) 
        {
            bullet.InitPlaced(moveTime, stopTime, placedStopSpeed);
            bullet.CheckOutBound = this.CheckOutBound;
            bullet.Create(muzzle.position, muzzle.rotation, isCalculatedDamage, speed,
                    speedRate, angle, angleRate, isCritical, startDistance, lifeTime);
        }
    }

    void AimingShoot() 
    {
        if (stopAttackCooltime) 
        {
            targetfixedAngle = AngleBetweenTransform(muzzle, targetTransform);
            stopAttackCooltime = false;
        }
        angle = targetfixedAngle;
        NwayShoot(speed, speedRate, -angle + 180, angleRate, angleRange, count);
        stopAttackCooltime = true;
    }

    void SpreadingShoot() 
    {
        for (int i = 0; i < groupCount; i++) 
        {
            NwayShoot(speed + groupSpeed * i, speedRate, angle, angleRate, angleRange, count);
        }
    }

    void RandomSpreadingShoot() 
    {
        for (int i = 0; i < count; i++) 
        {
            BasicShoot(speed + speedRange * UnityEngine.Random.Range(0, 1.0f), 0,
                angle + angleRange * (UnityEngine.Random.Range(0, 1.0f) - 0.5f), 0, muzzle.position);
        }
    }

    void OvertakingShoot() 
    {
        StartCoroutine("CoOvertakingShoot");
    }

    IEnumerator CoOvertakingShoot() 
    {
        for (int i = 0; i < groupCount; i++) 
        {
            NwayShoot(speed + groupSpeed * i, 0, angle + groupAngle * i, 0, angleRange, count);

            yield return new WaitForSeconds(groupInterval);
        }
    }

    void CrossShoot() 
    {
        // ShapeBullet bullet = HSPoolManager.Instance.NewItem<ShapeBullet>(bulletType.ToString());
        // if (bullet) 
        // {
        //     bullet.Create(muzzle.position, muzzle.rotation, isCalculatedDamage,
        //             speed, isCritical);
        // }
    }

    void ArcShoot() 
    {
       ArcBullet bullet = HSPoolManager.Instance.NewItem<ArcBullet>(bulletType.ToString());
        if (bullet) 
        {
            bullet.Create(muzzle.position, targetTransform.position, isCalculatedDamage, arrivalTime, height);
        }
    }

    void PatternShoot() 
    {
        StartCoroutine(Patterns());
    }

    IEnumerator Patterns()
    {
        string[] lines = pattern.Split('\n');
        for (int y = lines.Length - 1; y >= 0; y--)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                char p = lines[y][x];
                if (p != ' ')
                {
                    float angles = angle - forwardAngleSpeed * ((float)x / (pat_width - 1) - 0.5f);
                    BasicShoot(speed, speedRate, angles, angleRate, muzzle.position);
                }
                
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void CircleShapeShoot()
    {
        CirclePatterns();
    }

    void CirclePatterns()
    {
        ShapeBullet centorbullet = HSPoolManager.Instance.NewItem<ShapeBullet>(bulletType.ToString());

        if (centorbullet) 
        {
            centorbullet.Init(angleSpeed, circleAngle, radius, vertexShape, segments);
            centorbullet.Create(shapeType, muzzle.position, muzzle.rotation, isCalculatedDamage, speed, angle, angleRate, lifeTime);
        }
    }

    void ShooterShoot()
    {
        ShooterBullet bullet = HSPoolManager.Instance.NewItem<ShooterBullet>(bulletType.ToString());
        if (bullet) 
        {
            bullet.Create(muzzle.position, muzzle.rotation, speed, angle);
        }
    }

    void GoblinFireShoot() {
        GoblinFireBullet bullet = HSPoolManager.Instance.NewItem<GoblinFireBullet>(bulletType.ToString());
        if (bullet) {
            bullet.Init(turnSpeed, targetTransform, homingSpeedRate, wavingRange, wavingCycle, decreaseHomingSpeed);
            bullet.CheckOutBound = this.CheckOutBound;
            bullet.Create(muzzle.position, muzzle.rotation, isCalculatedDamage,
                    speed, speedRate, angle, angleRate, isCritical, startDistance, lifeTime);
        }
    }


    void CustomShapeShoot() 
    {
        float dir = rot;

        for (int r = 0; r < vertex; r++) 
        {
            for (int i = 1; i <= m; i++) 
            {
                CreateCustomShapeBullet(muzzle.position, dir + cs_xx[i], cs_v[i] * speed / sup);
                CreateCustomShapeBullet(muzzle.position, dir - cs_xx[i], cs_v[i] * speed / sup);
                CreateCustomShapeBullet(muzzle.position, dir, speed);

                dir += 360 / vertex;
            }
        }
    }

    public void SetCustomShapeParameter(float rot, int vertex, float sup) 
    {
        this.rot = rot;
        this.vertex = vertex;
        this.sup = sup;

        InitCustomShape();
    }

    void CreateCustomShapeBullet(Vector3 startPos, float rot, float speed) 
    {
        Bullet bullet = HSPoolManager.Instance.NewItem<Bullet>(bulletType.ToString());
       
        if (bullet) 
        {
            bullet.CheckOutBound = this.CheckOutBound;

            bullet.Create(startPos, muzzle.rotation, isCalculatedDamage, speed,
                speedRate, rot, angleRate, isCritical, startDistance, lifeTime);

            if (rotateBulletCore) 
            {
                bullet.RotateBulletCore(new Vector3(0, rot, 0));
            }
        }
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) 
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    float GetAngle(Vector3 from, Vector3 to) 
    {
        Vector3 diff = to - from;
        return Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
    }

    float AngleBetweenTransform(Transform from, Transform to) 
    {
        Vector3 targetDir = to.position - from.position;
        Vector3 forward = from.forward;
        return Vector3.SignedAngle(targetDir, forward, Vector3.up);
    }

    public void StopAllCoroutine()
    {
        isRunning = false;
    }

    public void UpdateNwayOption(float angleRange, int count) 
    {
        this.angleRange = angleRange;
        this.count = count;
    }

    public void InitCustomShape() 
    {
        cs_v.Clear();
        cs_xx.Clear();

        m = (int)Mathf.Floor(sup / 2);
        a = 2 * Mathf.Sin(Mathf.PI / vertex);
        phi = ((Mathf.PI / 2f) * (vertex - 2f)) / vertex;
        cs_v.Add(0);
        cs_xx.Add(0);

        for (int i = 1; i <= m; i++) 
        {
            cs_v.Add(Mathf.Sqrt(sup * sup - 2 * a * Mathf.Cos(phi) * i * sup + a * a * i * i));
        }

        for (int i = 1; i <= m; i++) 
        {
            cs_xx.Add(Mathf.Rad2Deg * (Mathf.Asin(a * Mathf.Sin(phi) * i / cs_v[i])));
        }
    }


    public void AddBullet() 
    {
        switch (shootingType) 
        {
            case ShootingType.Multiple:
                count++;
                multipleRange += multipleStep;
                break;
            case ShootingType.Nway:
                count += 2;
                break;
            case ShootingType.RandomSpreading:
                count++;
                lifeTime += 0.05f;
                break;
            case ShootingType.RandomNway:
                count++;
                break;
        }
    }

    ShootFunc GetShootFunc(ShootingType type) 
    {
        switch (type) 
        {
            case ShootingType.Directional:
                return BasicShoot;
            case ShootingType.Forward:
                return ForwardShoot;
            case ShootingType.Nway:
                return NwayShoot;
            case ShootingType.Circle:
                return CircleShoot;
            case ShootingType.RandomNway:
                return RandomNwayShoot;
            case ShootingType.RandomCircle:
                return RandomCircleShoot;
            case ShootingType.Multiple:
                return MultipleShoot;
            case ShootingType.Homing:
                return HomingShoot;
            case ShootingType.DelayHoming:
                return DelayHomingShoot;
            case ShootingType.RandomHoming:
                return RandomHomingShoot;
            case ShootingType.RollingNway:
                return RollingNway;
            case ShootingType.WavingNway:
                return WavingNwayShoot;
            case ShootingType.CircleWavingNway:
                return WavingCircleShoot;
            case ShootingType.Placed:
                return PlacedShoot;
            case ShootingType.Aiming:
                return AimingShoot;
            case ShootingType.Spreading:
                return SpreadingShoot;
            case ShootingType.RandomSpreading:
                return RandomSpreadingShoot;
            case ShootingType.Overtaking:
                return OvertakingShoot;
            case ShootingType.Cross:
                return CrossShoot;
            case ShootingType.RandomDirectional:
                return RandomDirectionalShoot;
            case ShootingType.Arc:
                return ArcShoot;
            case ShootingType.CustomShape:
                return CustomShapeShoot;
            case ShootingType.Pattern:
                return PatternShoot;
            case ShootingType.CustomShapeForward:
                return CircleShapeShoot;
            case ShootingType.ShooterShooter:
                return ShooterShoot;
            case ShootingType.GoblinFire:
                return GoblinFireShoot;
            default:
                return null;
        }
    }

    private string GetJsonPath()
    {
        return Path.Combine(Application.dataPath, "Resources/Data", jsonName + ".json");
    }

   public void SaveParameters()
    {
        try
        {
            BaseParameters data = CreateParameters();

            // TypeNameHandling을 All로 설정하여 타입 정보를 포함
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
            File.WriteAllText(GetJsonPath(), json);
            Debug.Log("Save 완료");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error saving parameters: " + ex.Message);
        }
    }

    public void LoadParameters()
    {
        string path = GetJsonPathWithOpenFilePanel();
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            Debug.LogWarning("Save file not found");
            return;
        }

        try
        {
            string json = File.ReadAllText(path);

            // TypeNameHandling을 All로 설정하여 타입 정보를 포함
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            BaseParameters baseData = JsonConvert.DeserializeObject<BaseParameters>(json, settings);

            if (baseData != null)
            {
                Debug.Log("Load 완료");
                isRunning = false;
                RemoveType(shootingType);
                ApplyParameters(baseData);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error loading parameters: " + ex.Message);
        }
    }

     private string GetJsonPathWithOpenFilePanel()
    {
        #if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Select JSON file", Application.dataPath + "/Resources/Data", "json");
        return path;
        #else
        return null;
        #endif
    }

    private BaseParameters CreateParameters()
    {
        BaseParameters data = null;

        switch (shootingType)
        {
            case ShootingType.Forward:
            data = new ForwardParameters
            {
                forwardAngleSpeed = this.forwardAngleSpeed,
                angleRange = this.angleRange,
                count = this.count
            };
            break;
        case ShootingType.Nway:
            data = new NwayParameters
            {
                angleRange = this.angleRange,
                count = this.count
            };
            break;
        case ShootingType.Circle:
            data = new NwayParameters
            {
                angleRange = this.angleRange,
                count = this.count
            };
            break;
        case ShootingType.RandomNway:
            data = new ForwardParameters
            {
                forwardAngleSpeed = this.forwardAngleSpeed,
                angleRange = this.angleRange,
                count = this.count
            };
            break;
        case ShootingType.Multiple:
            data = new MultipleParameters
            {
                multipleStep = this.multipleStep,
                multipleRange = this.multipleRange
            };
            break;
         case ShootingType.CustomShape:
            data = new CustomShapeParameters
            {
                rot = this.rot,
                vertex = this.vertex,
                sup = this.sup,
                rotateBulletCore = this.rotateBulletCore,
                //rotateBulletOffset = this.rotateBulletOffset
            };
            break;
        case ShootingType.Homing:
            data = new HomingParameters
            {
                targetTransform = this.targetTransform,
                turnSpeed = this.turnSpeed,
                decreaseHomingSpeed = this.decreaseHomingSpeed,
                homingSpeedRate = this.homingSpeedRate,
            };
            break;
        case ShootingType.DelayHoming:
            data = new DelayHomingParameters
            {
                turnSpeed = this.turnSpeed,
                decreaseHomingSpeed = this.decreaseHomingSpeed,
                homingSpeedRate = this.homingSpeedRate,
                delayTime = this.delayTime,
            };
            break;
        case ShootingType.RandomHoming:
            data = new DelayHomingParameters
            {
                turnSpeed = this.turnSpeed,
                decreaseHomingSpeed = this.decreaseHomingSpeed,
                homingSpeedRate = this.homingSpeedRate,
            };
            break;
        case ShootingType.RollingNway:
            data = new RollingNwayParameters
            {
                forwardAngleSpeed = this.forwardAngleSpeed,
                angleRange = this.angleRange,
                count = this.count,
                nWayCount = this.nWayCount
            };
            break;
        case ShootingType.WavingNway:
            data = new WavingNwayParameters
            {
                forwardAngleSpeed = this.forwardAngleSpeed,
                angleRange = this.angleRange,
                count = this.count,
                wavingAngleRange = this.wavingAngleRange,
                cycle = this.cycle
            };
            break;
        case ShootingType.CircleWavingNway:
            data = new WavingNwayParameters
            {
                count = this.count,
                wavingAngleRange = this.wavingAngleRange,
                cycle = this.cycle
            };
            break;
        case ShootingType.Placed:
            data = new PlacedParameters
            {
                moveTime = this.moveTime,
                stopTime = this.stopTime,
                placedStopSpeed = this.placedStopSpeed,
            };
            break;
        case ShootingType.Aiming:
            data = new AimingParameters
            {
                angleRange = this.angleRange,
                count = this.count,
                targetTransform = this.targetTransform,
                stopAttackCooltime = this.stopAttackCooltime,
                targetfixedAngle = this.targetfixedAngle,
            };
            break;
        case ShootingType.Spreading:
            data = new SpreadingParameters
            {
                angleRange = this.angleRange,
                count = this.count,
                groupSpeed = this.groupSpeed,
                groupCount = this.groupCount
            };
            break;
        case ShootingType.RandomSpreading:
            data = new RandomSpreadingParameters
            {
                angleRange = this.angleRange,
                count = this.count,
                speedRange = this.speedRange
            };
            break;
        case ShootingType.Overtaking:
            data = new OvertakingParameters
            {
                angleRange = this.angleRange,
                count = this.count,
                groupSpeed = this.groupSpeed,
                groupCount = this.groupCount,
                groupAngle = this.groupAngle,
                groupInterval = this.groupInterval
            };
            break;
        case ShootingType.Arc:
            data = new ArcParameters
            {
                targetTransform = this.targetTransform,
                arrivalTime = this.arrivalTime,
                height = this.height
            };
            break;
         case ShootingType.CustomShapeForward:
            data = new CustomShapeForwardParameters
            {
                shapeType = this.shapeType,
                vertexShape = this.vertexShape,
                radius = this.radius,
                segments = this.segments,
                angleSpeed = this.angleSpeed,
                circleAngle = this.circleAngle
            };
            break;
            default:
                data = new BaseParameters();
                break;
        }

        // 공통 파라미터 설정
        data.shootingType = this.shootingType;
        data.bulletType = this.bulletType;
        data.power = this.power;
        data.speed = this.speed;
        data.speedRate = this.speedRate;
        data.angle = this.angle;
        data.angleRate = this.angleRate;
        data.startDistance = this.startDistance;
        data.lifeTime = this.lifeTime;
        data.attackCooltime = this.attackCooltime;
        data.attackTime = this.attackTime;
        data.stopAttackDelay = this.stopAttackDelay;
        data.attackTimeReset = this.attackTimeReset;
        data.attackFixedTime = this.attackFixedTime;

        return data;
    }

    public void ApplyParameters(BaseParameters data)
    {
        shootingType = data.shootingType;
        bulletType = data.bulletType;
        power = data.power;
        speed = data.speed;
        speedRate = data.speedRate;
        angle = data.angle;
        angleRate = data.angleRate;
        startDistance = data.startDistance;
        lifeTime = data.lifeTime;
        attackCooltime = data.attackCooltime;
        attackTime = data.attackTime;
        stopAttackDelay = data.stopAttackDelay;
        attackTimeReset = data.attackTimeReset;
        attackFixedTime = data.attackFixedTime;

        switch (data)
        {
            case ForwardParameters forwardData:
                forwardAngleSpeed = forwardData.forwardAngleSpeed;
                angleRange = forwardData.angleRange;
                count = forwardData.count;
                break;

            case NwayParameters nwayData:
                angleRange = nwayData.angleRange;
                count = nwayData.count;
                break;

            case MultipleParameters multipleData:
                multipleStep = multipleData.multipleStep;
                multipleRange = multipleData.multipleRange;
                break;

            case CustomShapeParameters customShapeData:
                rot = customShapeData.rot;
                vertex = customShapeData.vertex;
                sup = customShapeData.sup;
                rotateBulletCore = customShapeData.rotateBulletCore;
                //rotateBulletOffset = customShapeData.rotateBulletOffset;
                break;

            case HomingParameters homingData:
                targetTransform = homingData.targetTransform;
                turnSpeed = homingData.turnSpeed;
                decreaseHomingSpeed = homingData.decreaseHomingSpeed;
                homingSpeedRate = homingData.homingSpeedRate;
                break;

            case DelayHomingParameters delayHomingData:
                turnSpeed = delayHomingData.turnSpeed;
                decreaseHomingSpeed = delayHomingData.decreaseHomingSpeed;
                homingSpeedRate = delayHomingData.homingSpeedRate;
                delayTime = delayHomingData.delayTime;
                break;

            case RollingNwayParameters rollingNwayData:
                forwardAngleSpeed = rollingNwayData.forwardAngleSpeed;
                angleRange = rollingNwayData.angleRange;
                count = rollingNwayData.count;
                nWayCount = rollingNwayData.nWayCount;
                break;

            case WavingNwayParameters wavingNwayData:
                forwardAngleSpeed = wavingNwayData.forwardAngleSpeed;
                angleRange = wavingNwayData.angleRange;
                count = wavingNwayData.count;
                wavingAngleRange = wavingNwayData.wavingAngleRange;
                cycle = wavingNwayData.cycle;
                break;

            case PlacedParameters placedData:
                moveTime = placedData.moveTime;
                stopTime = placedData.stopTime;
                placedStopSpeed = placedData.placedStopSpeed;
                break;

            case AimingParameters aimingData:
                angleRange = aimingData.angleRange;
                count = aimingData.count;
                targetTransform = aimingData.targetTransform;
                stopAttackCooltime = aimingData.stopAttackCooltime;
                targetfixedAngle = aimingData.targetfixedAngle;
                break;

            case SpreadingParameters spreadingData:
                angleRange = spreadingData.angleRange;
                count = spreadingData.count;
                groupSpeed = spreadingData.groupSpeed;
                groupCount = spreadingData.groupCount;
                break;

            case RandomSpreadingParameters randomSpreadingData:
                angleRange = randomSpreadingData.angleRange;
                count = randomSpreadingData.count;
                speedRange = randomSpreadingData.speedRange;
                break;

            case OvertakingParameters overtakingData:
                angleRange = overtakingData.angleRange;
                count = overtakingData.count;
                groupSpeed = overtakingData.groupSpeed;
                groupCount = overtakingData.groupCount;
                groupAngle = overtakingData.groupAngle;
                groupInterval = overtakingData.groupInterval;
                break;

            case ArcParameters arcData:
                targetTransform = arcData.targetTransform;
                arrivalTime = arcData.arrivalTime;
                height = arcData.height;
                break;

            case CustomShapeForwardParameters customShapeForwardData:
                shapeType = customShapeForwardData.shapeType;
                vertexShape = customShapeForwardData.vertexShape;
                radius = customShapeForwardData.radius;
                segments = customShapeForwardData.segments;
                angleSpeed = customShapeForwardData.angleSpeed;
                circleAngle = customShapeForwardData.circleAngle;
                break;
        }

        SetType(shootingType);
    }

}
