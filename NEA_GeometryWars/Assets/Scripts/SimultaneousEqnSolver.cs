public class SimultaneousEqnSolver
{
    //eqn has to be of form "ax + by = c"
    private float[] Eqn1Coefficients = new float[3]; 
    private float[] Eqn2Coefficients = new float[3];

    private float y;
    private float x;

    private float TempForEqn1xCoeffcient;

    public SimultaneousEqnSolver(float[] Their1stEqn, float[] Their2ndEqn)
    {
        Eqn1Coefficients = Their1stEqn;
        Eqn2Coefficients = Their2ndEqn;
        TempForEqn1xCoeffcient = Their1stEqn[0];
    }

    private void SolveSimulEqn()
    {
        //selecting coefficient of x to be same
        for(int i = 0; i < Eqn1Coefficients.Length; i++)
        {
            Eqn1Coefficients[i] *= Eqn2Coefficients[0];
        }

        for(int i = 0; i < Eqn2Coefficients.Length; i++)
        {
            Eqn2Coefficients[i] *= TempForEqn1xCoeffcient;
        }

        //as the coefficient of X is the same, i just ignored it and subtract each of the other terms.
        float CoefficientOfY = Eqn1Coefficients[1] - Eqn2Coefficients[1];
        float CoefficientYTimesYEquals = Eqn1Coefficients[2] - Eqn2Coefficients[2];
        y = CoefficientYTimesYEquals / CoefficientOfY;

        //x = (c-by)/a
        x = (Eqn1Coefficients[2] - Eqn1Coefficients[1] * y) / Eqn1Coefficients[0];
        
    }

    public bool EnemyGoingAwayFromBullet()
    {
        SolveSimulEqn();

        //if either just the x or y is negative then it means the enemy is moving away from the bullet.
        bool is_enemey_moving_away = x < 0 || y < 0;
        return is_enemey_moving_away;
    }
} 