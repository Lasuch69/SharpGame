namespace SharpGame;

[GlobalClass]
public sealed partial class ProjectileComponent : Node
{
	[Export(PropertyHint.File)]
	public string ScenePath { get; set; }

	string _path;
	PackedScene _scene;

	public Projectile SpawnProjectile(Vector2 position, Vector2 velocity, Node parent)
	{
		if (_path != ScenePath)
		{
			_scene = ResourceLoader.Load<PackedScene>(ScenePath);
			_path = ScenePath;
		}

		if (_scene is null)
		{
			GD.PrintErr("Can't spawn projectile! Scene is null!");
			return null;
		}

		Node scene = _scene.Instantiate();

		if (scene is not Projectile)
		{
			GD.PrintErr(String.Format("Can't spawn projectile! Invalid class! ({0})", scene.GetClass()));
			return null;
		}

		Projectile projectile = (Projectile)scene;

		projectile.Position = position;
		projectile.Velocity = velocity;

		parent.AddChild(projectile);
		return projectile;
	}
}
