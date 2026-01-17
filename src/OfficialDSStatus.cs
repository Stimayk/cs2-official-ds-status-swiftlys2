using SwiftlyS2.Shared;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Plugins;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace OfficialDSStatus;

[PluginMetadata(
    Id = "OfficialDSStatus",
    Version = "1.0.0",
    Name = "OfficialDSStatus",
    Author = "E!N",
    Website = "https://nova-hosting.ru/?ref=ein"
)]
public sealed class OfficialDsStatus(ISwiftlyCore core) : BasePlugin(core)
{
    private Guid? _onEventRoundStartId;

    public override void Load(bool hotReload)
    {
        _onEventRoundStartId = Core.GameEvent.HookPre<EventRoundStart>(OnRoundStart);
    }

    public override void Unload()
    {
        if (!_onEventRoundStartId.HasValue) return;
        Core.GameEvent.Unhook(_onEventRoundStartId.Value);
        _onEventRoundStartId = null;
    }

    private HookResult OnRoundStart(EventRoundStart _)
    {
        var gameRulesProxy = Core.EntitySystem
            .GetAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules")
            .FirstOrDefault();

        if (gameRulesProxy?.GameRules is { } gameRules) gameRules.IsValveDS = true;

        return HookResult.Continue;
    }
}