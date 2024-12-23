using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("NoEffects", "RustFlash", "2.0.0")]
    [Description("Allows disabling specific effects for players with permissions")]
    public class NoEffects : RustPlugin
    {
        private const string PermPrefix = "noeffects.";
        private static readonly string[] EffectTypes = { "bleeding", "freezing", "overheating", "wet", "radiation" };

        private void Init()
        {
            foreach (var effect in EffectTypes)
            {
                permission.RegisterPermission(PermPrefix + effect, this);
            }
        }

        private object OnRunPlayerMetabolism(PlayerMetabolism metabolism, BasePlayer player, float delta)
        {
            if (player == null || metabolism == null) return null;

            var userId = player.UserIDString;

            if (permission.UserHasPermission(userId, PermPrefix + "bleeding"))
                metabolism.bleeding.value = 0f;

            if (permission.UserHasPermission(userId, PermPrefix + "freezing") && metabolism.temperature.value < 35)
                metabolism.temperature.value = 35;

            if (permission.UserHasPermission(userId, PermPrefix + "overheating") && metabolism.temperature.value > 39)
                metabolism.temperature.value = 39;

            if (permission.UserHasPermission(userId, PermPrefix + "wet"))
                metabolism.wetness.value = 0;

            if (permission.UserHasPermission(userId, PermPrefix + "radiation"))
            {
                metabolism.radiation_level.value = 0;
                metabolism.radiation_poison.value = 0;
            }

            return null;
        }
    }
}