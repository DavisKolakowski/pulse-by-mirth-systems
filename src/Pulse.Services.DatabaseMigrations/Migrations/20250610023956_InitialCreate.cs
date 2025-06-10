using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pulse.Services.DatabaseMigrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:follow_type", "tag,venue")
                .Annotation("Npgsql:Enum:media_type", "photo,video")
                .Annotation("Npgsql:Enum:notification_type", "new_special,special_reminder,venue_activity,tag_activity,system_alert,welcome")
                .Annotation("Npgsql:PostgresExtension:address_standardizer", ",,")
                .Annotation("Npgsql:PostgresExtension:address_standardizer_data_us", ",,")
                .Annotation("Npgsql:PostgresExtension:fuzzystrmatch", ",,")
                .Annotation("Npgsql:PostgresExtension:plpgsql", ",,")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .Annotation("Npgsql:PostgresExtension:postgis_raster", ",,")
                .Annotation("Npgsql:PostgresExtension:postgis_sfcgal", ",,")
                .Annotation("Npgsql:PostgresExtension:postgis_tiger_geocoder", ",,")
                .Annotation("Npgsql:PostgresExtension:postgis_topology", ",,");

            migrationBuilder.CreateTable(
                name: "days_of_week",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    short_name = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    iso_number = table.Column<int>(type: "integer", nullable: false),
                    is_weekday = table.Column<bool>(type: "boolean", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_days_of_week", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    display_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    resource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    display_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "special_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    icon = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_special_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    icon = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    usage_count = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    updated_by_user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    deactivated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    deactivated_by_user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    provider_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    display_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    profile_picture = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    last_login_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    deactivated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    deactivated_by_user_id = table.Column<long>(type: "bigint", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venue_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    icon = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venue_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vibes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    icon = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    usage_count = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    updated_by_user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    deactivated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    deactivated_by_user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vibes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    permission_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "fk_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    assigned_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    assigned_by_user_id = table.Column<long>(type: "bigint", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    deactivated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    deactivated_by_user_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_assigned_by_user_id",
                        column: x => x.assigned_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_user_roles_users_deactivated_by_user_id",
                        column: x => x.deactivated_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "venues",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    website = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    profile_image = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    street_address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    secondary_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    locality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    postal_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    location = table.Column<Point>(type: "geography (point)", nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    updated_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    deactivated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    deactivated_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venues", x => x.id);
                    table.ForeignKey(
                        name: "fk_venues_venue_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "venue_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "activity_threads",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    venue_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_activity_threads", x => x.id);
                    table.ForeignKey(
                        name: "fk_activity_threads_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "business_hours",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    venue_id = table.Column<long>(type: "bigint", nullable: false),
                    day_of_week_id = table.Column<int>(type: "integer", nullable: false),
                    open_time = table.Column<LocalTime>(type: "time", nullable: true),
                    close_time = table.Column<LocalTime>(type: "time", nullable: true),
                    is_closed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_business_hours", x => x.id);
                    table.ForeignKey(
                        name: "fk_business_hours_day_of_weeks_day_of_week_id",
                        column: x => x.day_of_week_id,
                        principalTable: "days_of_week",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_business_hours_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "specials",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    venue_id = table.Column<long>(type: "bigint", nullable: false),
                    special_category_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    start_date = table.Column<LocalDate>(type: "date", nullable: false),
                    start_time = table.Column<LocalTime>(type: "time", nullable: false),
                    end_time = table.Column<LocalTime>(type: "time", nullable: true),
                    end_date = table.Column<LocalDate>(type: "date", nullable: true),
                    is_recurring = table.Column<bool>(type: "boolean", nullable: false),
                    cron_schedule = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    updated_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    deactivated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    deactivated_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_specials", x => x.id);
                    table.ForeignKey(
                        name: "fk_specials_special_categories_special_category_id",
                        column: x => x.special_category_id,
                        principalTable: "special_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_specials_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_follows",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    follow_type = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<long>(type: "bigint", nullable: true),
                    venue_id = table.Column<long>(type: "bigint", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    unfollowed_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    notify_new_specials = table.Column<bool>(type: "boolean", nullable: false),
                    notify_special_reminders = table.Column<bool>(type: "boolean", nullable: false),
                    notify_venue_activity = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_follows", x => x.id);
                    table.CheckConstraint("CK_UserFollow_Association", "(follow_type = 1 AND tag_id IS NOT NULL AND venue_id IS NULL) OR (follow_type = 2 AND tag_id IS NULL AND venue_id IS NOT NULL)");
                    table.ForeignKey(
                        name: "fk_user_follows_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_follows_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_follows_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "venue_roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    venue_id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    assigned_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    assigned_by_user_id = table.Column<long>(type: "bigint", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    deactivated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    deactivated_by_user_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venue_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_venue_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_venue_roles_users_assigned_by_user_id",
                        column: x => x.assigned_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_venue_roles_users_deactivated_by_user_id",
                        column: x => x.deactivated_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_venue_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_venue_roles_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    activity_thread_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    deleted_by_user_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_posts", x => x.id);
                    table.ForeignKey(
                        name: "fk_posts_activity_threads_activity_thread_id",
                        column: x => x.activity_thread_id,
                        principalTable: "activity_threads",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_posts_users_deleted_by_user_id",
                        column: x => x.deleted_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_posts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    recipient_user_id = table.Column<long>(type: "bigint", nullable: false),
                    sender_user_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    action_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    read_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    related_venue_id = table.Column<long>(type: "bigint", nullable: true),
                    related_special_id = table.Column<long>(type: "bigint", nullable: true),
                    related_tag_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_specials_related_special_id",
                        column: x => x.related_special_id,
                        principalTable: "specials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_notifications_tags_related_tag_id",
                        column: x => x.related_tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_notifications_users_recipient_user_id",
                        column: x => x.recipient_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notifications_users_sender_user_id",
                        column: x => x.sender_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_notifications_venues_related_venue_id",
                        column: x => x.related_venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "special_tags",
                columns: table => new
                {
                    special_id = table.Column<long>(type: "bigint", nullable: false),
                    tag_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_special_tags", x => new { x.special_id, x.tag_id });
                    table.ForeignKey(
                        name: "fk_special_tags_specials_special_id",
                        column: x => x.special_id,
                        principalTable: "specials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_special_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "media",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    file_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    content_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    width = table.Column<int>(type: "integer", nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    duration_seconds = table.Column<int>(type: "integer", nullable: true),
                    alt_text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    venue_id = table.Column<long>(type: "bigint", nullable: true),
                    post_id = table.Column<long>(type: "bigint", nullable: true),
                    media_type = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    deleted_by_user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media", x => x.id);
                    table.CheckConstraint("CK_Media_Association", "(venue_id IS NOT NULL AND post_id IS NULL) OR (venue_id IS NULL AND post_id IS NOT NULL)");
                    table.ForeignKey(
                        name: "fk_media_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_media_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_vibes",
                columns: table => new
                {
                    post_id = table.Column<long>(type: "bigint", nullable: false),
                    vibe_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_post_vibes", x => new { x.post_id, x.vibe_id });
                    table.ForeignKey(
                        name: "fk_post_vibes_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_post_vibes_vibes_vibe_id",
                        column: x => x.vibe_id,
                        principalTable: "vibes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "days_of_week",
                columns: new[] { "id", "is_weekday", "iso_number", "name", "short_name", "sort_order" },
                values: new object[,]
                {
                    { 1, false, 0, "Sunday", "SUN", 1 },
                    { 2, true, 1, "Monday", "MON", 2 },
                    { 3, true, 2, "Tuesday", "TUE", 3 },
                    { 4, true, 3, "Wednesday", "WED", 4 },
                    { 5, true, 4, "Thursday", "THU", 5 },
                    { 6, true, 5, "Friday", "FRI", 6 },
                    { 7, false, 6, "Saturday", "SAT", 7 }
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "action", "category", "created_at", "description", "display_name", "is_active", "name", "resource", "sort_order" },
                values: new object[,]
                {
                    { 1, "read", "Venue", NodaTime.Instant.FromUnixTimeTicks(0L), "Query and view venue information", "Read Venues", true, "read:venues", "venues", 1 },
                    { 2, "write", "Venue", NodaTime.Instant.FromUnixTimeTicks(0L), "Create and update venue information", "Write Venues", true, "write:venues", "venues", 2 },
                    { 3, "delete", "Venue", NodaTime.Instant.FromUnixTimeTicks(0L), "Delete venues from the system", "Delete Venues", true, "delete:venues", "venues", 3 },
                    { 4, "write", "Special", NodaTime.Instant.FromUnixTimeTicks(0L), "Create and update special offers", "Write Specials", true, "write:specials", "specials", 4 },
                    { 5, "delete", "Special", NodaTime.Instant.FromUnixTimeTicks(0L), "Delete special offers", "Delete Specials", true, "delete:specials", "specials", 5 },
                    { 6, "read", "Content", NodaTime.Instant.FromUnixTimeTicks(0L), "Read all user-generated content and posts", "Read Content", true, "read:content", "content", 6 },
                    { 7, "write", "Content", NodaTime.Instant.FromUnixTimeTicks(0L), "Create and update content across all venues", "Write Content", true, "write:content", "content", 7 },
                    { 8, "delete", "Content", NodaTime.Instant.FromUnixTimeTicks(0L), "Delete inappropriate or violating content", "Delete Content", true, "delete:content", "content", 8 },
                    { 9, "moderate", "Content", NodaTime.Instant.FromUnixTimeTicks(0L), "Moderate user posts and venue content", "Moderate Content", true, "moderate:content", "content", 9 },
                    { 10, "write", "Post", NodaTime.Instant.FromUnixTimeTicks(0L), "Create posts in venue activity threads", "Write Posts", true, "write:posts", "posts", 10 },
                    { 11, "delete", "Post", NodaTime.Instant.FromUnixTimeTicks(0L), "Delete posts from activity threads", "Delete Posts", true, "delete:posts", "posts", 11 },
                    { 12, "moderate", "Post", NodaTime.Instant.FromUnixTimeTicks(0L), "Moderate posts and manage thread activity", "Moderate Posts", true, "moderate:posts", "posts", 12 },
                    { 13, "upload", "Media", NodaTime.Instant.FromUnixTimeTicks(0L), "Upload photos and videos to venue profiles and posts", "Upload Media", true, "upload:media", "media", 13 },
                    { 14, "delete", "Media", NodaTime.Instant.FromUnixTimeTicks(0L), "Delete media content", "Delete Media", true, "delete:media", "media", 14 },
                    { 15, "moderate", "Media", NodaTime.Instant.FromUnixTimeTicks(0L), "Moderate media content for appropriateness", "Moderate Media", true, "moderate:media", "media", 15 },
                    { 16, "read", "Tag", NodaTime.Instant.FromUnixTimeTicks(0L), "Read tag definitions and assignments", "Read Tags", true, "read:tags", "tags", 16 },
                    { 17, "write", "Tag", NodaTime.Instant.FromUnixTimeTicks(0L), "Create and update tags for specials", "Write Tags", true, "write:tags", "tags", 17 },
                    { 18, "delete", "Tag", NodaTime.Instant.FromUnixTimeTicks(0L), "Delete or consolidate tags", "Delete Tags", true, "delete:tags", "tags", 18 },
                    { 19, "moderate", "Tag", NodaTime.Instant.FromUnixTimeTicks(0L), "Feature, hide, or manage tag usage across platform", "Moderate Tags", true, "moderate:tags", "tags", 19 },
                    { 20, "read", "Vibe", NodaTime.Instant.FromUnixTimeTicks(0L), "Read vibe definitions and current venue vibes", "Read Vibes", true, "read:vibes", "vibes", 20 },
                    { 21, "write", "Vibe", NodaTime.Instant.FromUnixTimeTicks(0L), "Create vibes in user posts", "Write Vibes", true, "write:vibes", "vibes", 21 },
                    { 22, "moderate", "Vibe", NodaTime.Instant.FromUnixTimeTicks(0L), "Moderate vibe content for appropriateness", "Moderate Vibes", true, "moderate:vibes", "vibes", 22 },
                    { 23, "read", "VenueCategory", NodaTime.Instant.FromUnixTimeTicks(0L), "Read available venue category classifications", "Read Venue Categories", true, "read:venue-categories", "venue-categories", 23 },
                    { 24, "write", "VenueCategory", NodaTime.Instant.FromUnixTimeTicks(0L), "Create and update venue category definitions", "Write Venue Categories", true, "write:venue-categories", "venue-categories", 24 },
                    { 25, "delete", "VenueCategory", NodaTime.Instant.FromUnixTimeTicks(0L), "Remove venue categories from the system", "Delete Venue Categories", true, "delete:venue-categories", "venue-categories", 25 },
                    { 26, "read", "Analytics", NodaTime.Instant.FromUnixTimeTicks(0L), "Access venue analytics and performance metrics", "Read Analytics", true, "read:analytics", "analytics", 26 },
                    { 27, "read", "Analytics", NodaTime.Instant.FromUnixTimeTicks(0L), "Access global platform analytics and insights", "Global Analytics", true, "read:analytics-global", "analytics-global", 27 },
                    { 28, "read", "Notification", NodaTime.Instant.FromUnixTimeTicks(0L), "Read user notifications", "Read Notifications", true, "read:notifications", "notifications", 28 },
                    { 29, "write", "Notification", NodaTime.Instant.FromUnixTimeTicks(0L), "Send notifications to users", "Write Notifications", true, "write:notifications", "notifications", 29 },
                    { 30, "manage", "Follow", NodaTime.Instant.FromUnixTimeTicks(0L), "Follow/unfollow tags and venues for notifications", "Manage Follows", true, "manage:follows", "follows", 30 },
                    { 31, "read", "VenueUser", NodaTime.Instant.FromUnixTimeTicks(0L), "Read venue user assignments and permissions", "Read Venue Users", true, "read:venue-users", "venue-users", 31 },
                    { 32, "write", "VenueUser", NodaTime.Instant.FromUnixTimeTicks(0L), "Assign users to venues and manage venue-specific permissions", "Write Venue Users", true, "write:venue-users", "venue-users", 32 },
                    { 33, "delete", "VenueUser", NodaTime.Instant.FromUnixTimeTicks(0L), "Remove users from venue assignments", "Delete Venue Users", true, "delete:venue-users", "venue-users", 33 },
                    { 34, "admin", "System", NodaTime.Instant.FromUnixTimeTicks(0L), "Full system administration access", "System Admin", true, "admin:system", "system", 34 },
                    { 35, "config", "System", NodaTime.Instant.FromUnixTimeTicks(0L), "Modify system configuration and settings", "System Config", true, "config:system", "system", 35 }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "created_at", "description", "display_name", "is_active", "name", "sort_order" },
                values: new object[,]
                {
                    { 1, NodaTime.Instant.FromUnixTimeTicks(0L), "Full global application administration access with complete system control", "Administrator", true, "administrator", 1 },
                    { 2, NodaTime.Instant.FromUnixTimeTicks(0L), "Manage all venues, content, and platform-wide moderation", "Content Manager", true, "content-manager", 2 },
                    { 3, NodaTime.Instant.FromUnixTimeTicks(0L), "Full management access for assigned venues including user management", "Venue Owner", true, "venue-owner", 3 },
                    { 4, NodaTime.Instant.FromUnixTimeTicks(0L), "Manage specials and content for assigned venues", "Venue Manager", true, "venue-manager", 4 }
                });

            migrationBuilder.InsertData(
                table: "special_categories",
                columns: new[] { "id", "description", "icon", "name", "sort_order" },
                values: new object[,]
                {
                    { 1, "Food specials, appetizers, and meal deals", "🍔", "Food", 1 },
                    { 2, "Drink specials, happy hours, and beverage promotions", "🍺", "Drink", 2 },
                    { 3, "Live music, DJs, trivia, karaoke, and other events", "🎵", "Entertainment", 3 }
                });

            migrationBuilder.InsertData(
                table: "tags",
                columns: new[] { "id", "color", "created_at", "created_by_user_id", "deactivated_at", "deactivated_by_user_id", "description", "icon", "is_active", "name", "updated_at", "updated_by_user_id", "usage_count" },
                values: new object[,]
                {
                    { 1L, "#FF6B35", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Happy hour drink specials", "🍻", true, "happyhour", null, null, 0 },
                    { 2L, "#4ECDC4", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Live music performances", "🎵", true, "livemusic", null, null, 0 },
                    { 3L, "#45B7D1", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Trivia nights and quiz events", "🧠", true, "trivia", null, null, 0 },
                    { 4L, "#FFA07A", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Karaoke nights", "🎤", true, "karaoke", null, null, 0 },
                    { 5L, "#98D8C8", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Food discounts and meal deals", "🍽️", true, "foodspecial", null, null, 0 }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "deactivated_at", "deactivated_by_user_id", "display_name", "email", "first_name", "is_active", "last_login_at", "last_name", "phone_number", "profile_picture", "provider_id", "updated_at" },
                values: new object[] { 1L, NodaTime.Instant.FromUnixTimeTicks(0L), null, null, "davis_kolakowski", "davis_kolakowski@mirthsystems.com", "Davis", true, null, "Kolakowski", null, "https://s.gravatar.com/avatar/a23b5565a17780ebec6bc03fccd4d0f6?s=480&r=pg&d=https%3A%2F%2Fcdn.auth0.com%2Favatars%2Fda.png", "auth0|682e1f2e2121380bbeb56dcf", null });

            migrationBuilder.InsertData(
                table: "venue_categories",
                columns: new[] { "id", "description", "icon", "name", "sort_order" },
                values: new object[,]
                {
                    { 1, "Dining establishments offering food and beverages", "🍽️", "Restaurant", 1 },
                    { 2, "Venues focused on drinks and nightlife", "🍸", "Bar", 2 },
                    { 3, "Casual spots for coffee and light meals", "☕", "Cafe", 3 },
                    { 4, "Venues for dancing and late-night entertainment", "🪩", "Nightclub", 4 },
                    { 5, "Casual venues with food, drinks, and often live music", "🍺", "Pub", 5 },
                    { 6, "Venues producing wine, offering tastings, food pairings, and live music", "🍷", "Winery", 6 },
                    { 7, "Venues brewing their own beer, often with food and live music", "🍻", "Brewery", 7 },
                    { 9, "Sophisticated venues with cocktails, small plates, and live music", "🛋️", "Lounge", 8 },
                    { 10, "Intimate dining venues with quality food, wine, and occasional live music", "🥂", "Bistro", 9 }
                });

            migrationBuilder.InsertData(
                table: "vibes",
                columns: new[] { "id", "color", "created_at", "created_by_user_id", "deactivated_at", "deactivated_by_user_id", "description", "icon", "is_active", "name", "updated_at", "updated_by_user_id", "usage_count" },
                values: new object[,]
                {
                    { 1L, "#FF6B35", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "High energy, crowded atmosphere", "🔥", true, "busy", null, null, 0 },
                    { 2L, "#4ECDC4", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Relaxed, laid-back atmosphere", "😌", true, "chill", null, null, 0 },
                    { 3L, "#45B7D1", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Fun and energetic atmosphere", "🎉", true, "lively", null, null, 0 },
                    { 4L, "#A8E6CF", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Peaceful and calm atmosphere", "🤫", true, "quiet", null, null, 0 },
                    { 5L, "#FFB6C1", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Perfect for couples and intimate conversations", "💕", true, "romantic", null, null, 0 }
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "permission_id", "role_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 },
                    { 8, 1 },
                    { 9, 1 },
                    { 10, 1 },
                    { 11, 1 },
                    { 12, 1 },
                    { 13, 1 },
                    { 14, 1 },
                    { 15, 1 },
                    { 16, 1 },
                    { 17, 1 },
                    { 18, 1 },
                    { 19, 1 },
                    { 20, 1 },
                    { 21, 1 },
                    { 22, 1 },
                    { 23, 1 },
                    { 24, 1 },
                    { 25, 1 },
                    { 26, 1 },
                    { 27, 1 },
                    { 28, 1 },
                    { 29, 1 },
                    { 30, 1 },
                    { 31, 1 },
                    { 32, 1 },
                    { 33, 1 },
                    { 34, 1 },
                    { 35, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 4, 2 },
                    { 6, 2 },
                    { 7, 2 },
                    { 8, 2 },
                    { 9, 2 },
                    { 10, 2 },
                    { 11, 2 },
                    { 12, 2 },
                    { 13, 2 },
                    { 14, 2 },
                    { 15, 2 },
                    { 16, 2 },
                    { 17, 2 },
                    { 19, 2 },
                    { 20, 2 },
                    { 22, 2 },
                    { 23, 2 },
                    { 26, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 3, 3 },
                    { 4, 3 },
                    { 5, 3 },
                    { 6, 3 },
                    { 7, 3 },
                    { 10, 3 },
                    { 13, 3 },
                    { 16, 3 },
                    { 17, 3 },
                    { 20, 3 },
                    { 21, 3 },
                    { 23, 3 },
                    { 26, 3 },
                    { 28, 3 },
                    { 29, 3 },
                    { 30, 3 },
                    { 31, 3 },
                    { 32, 3 },
                    { 33, 3 },
                    { 1, 4 },
                    { 4, 4 },
                    { 5, 4 },
                    { 6, 4 },
                    { 7, 4 },
                    { 10, 4 },
                    { 13, 4 },
                    { 16, 4 },
                    { 17, 4 },
                    { 20, 4 },
                    { 21, 4 },
                    { 23, 4 },
                    { 26, 4 },
                    { 28, 4 }
                });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "assigned_at", "assigned_by_user_id", "deactivated_at", "deactivated_by_user_id", "is_active", "role_id", "user_id" },
                values: new object[] { 1L, NodaTime.Instant.FromUnixTimeTicks(0L), null, null, null, true, 1, 1L });

            migrationBuilder.InsertData(
                table: "venues",
                columns: new[] { "id", "category_id", "country", "created_at", "created_by_user_id", "deactivated_at", "deactivated_by_user_id", "description", "email", "is_active", "locality", "location", "name", "phone_number", "postal_code", "profile_image", "region", "secondary_address", "street_address", "updated_at", "updated_by_user_id", "website" },
                values: new object[,]
                {
                    { 1L, 7, "United States", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Local craft brewery featuring house-made beers, pub fare, and live entertainment in a cozy atmosphere.", "info@bullfrogbrewery.com", true, "Williamsport", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-77.0057192 41.240432)"), "Bullfrog Brewery", "(570) 326-4700", "17701", null, "PA", null, "229 W 4th St", null, null, "https://bullfrogbrewery.com" },
                    { 2L, 1, "United States", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Family-friendly restaurant and ale house serving American cuisine with a great selection of craft beers and cocktails.", "info@thebrickyard.net", true, "Williamsport", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-77.0037646 41.2409825)"), "The Brickyard Restaurant & Ale House", "(570) 322-3876", "17701", null, "PA", null, "343 Pine St", null, null, "https://thebrickyard.net" },
                    { 3L, 2, "United States", NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, "Upscale gastropub featuring craft cocktails, local beers, and elevated bar food in a sophisticated atmosphere.", "info@thecrookedgoose.com", true, "Williamsport", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-77.0047521 41.2407201)"), "The Crooked Goose", "(570) 360-7435", "17701", null, "PA", null, "215 W 4th St", null, null, "https://thecrookedgoose.com" }
                });

            migrationBuilder.InsertData(
                table: "business_hours",
                columns: new[] { "id", "close_time", "day_of_week_id", "open_time", "venue_id" },
                values: new object[,]
                {
                    { 1L, new NodaTime.LocalTime(15, 0), 1, new NodaTime.LocalTime(10, 0), 1L },
                    { 2L, new NodaTime.LocalTime(22, 0), 2, new NodaTime.LocalTime(11, 30), 1L },
                    { 3L, new NodaTime.LocalTime(22, 0), 3, new NodaTime.LocalTime(11, 30), 1L },
                    { 4L, new NodaTime.LocalTime(22, 0), 4, new NodaTime.LocalTime(11, 30), 1L },
                    { 5L, new NodaTime.LocalTime(22, 0), 5, new NodaTime.LocalTime(11, 30), 1L },
                    { 6L, new NodaTime.LocalTime(0, 0), 6, new NodaTime.LocalTime(11, 30), 1L },
                    { 7L, new NodaTime.LocalTime(0, 0), 7, new NodaTime.LocalTime(11, 30), 1L },
                    { 8L, new NodaTime.LocalTime(23, 0), 1, new NodaTime.LocalTime(11, 0), 2L },
                    { 9L, new NodaTime.LocalTime(0, 0), 2, new NodaTime.LocalTime(11, 0), 2L },
                    { 10L, new NodaTime.LocalTime(0, 0), 3, new NodaTime.LocalTime(11, 0), 2L },
                    { 11L, new NodaTime.LocalTime(0, 0), 4, new NodaTime.LocalTime(11, 0), 2L },
                    { 12L, new NodaTime.LocalTime(0, 0), 5, new NodaTime.LocalTime(11, 0), 2L },
                    { 13L, new NodaTime.LocalTime(2, 0), 6, new NodaTime.LocalTime(11, 0), 2L },
                    { 14L, new NodaTime.LocalTime(2, 0), 7, new NodaTime.LocalTime(11, 0), 2L },
                    { 15L, new NodaTime.LocalTime(14, 0), 1, new NodaTime.LocalTime(10, 0), 3L }
                });

            migrationBuilder.InsertData(
                table: "business_hours",
                columns: new[] { "id", "close_time", "day_of_week_id", "is_closed", "open_time", "venue_id" },
                values: new object[] { 16L, null, 2, true, null, 3L });

            migrationBuilder.InsertData(
                table: "business_hours",
                columns: new[] { "id", "close_time", "day_of_week_id", "open_time", "venue_id" },
                values: new object[,]
                {
                    { 17L, new NodaTime.LocalTime(21, 0), 3, new NodaTime.LocalTime(11, 0), 3L },
                    { 18L, new NodaTime.LocalTime(21, 0), 4, new NodaTime.LocalTime(11, 0), 3L },
                    { 19L, new NodaTime.LocalTime(21, 0), 5, new NodaTime.LocalTime(11, 0), 3L },
                    { 20L, new NodaTime.LocalTime(22, 0), 6, new NodaTime.LocalTime(11, 0), 3L },
                    { 21L, new NodaTime.LocalTime(22, 0), 7, new NodaTime.LocalTime(11, 0), 3L }
                });

            migrationBuilder.InsertData(
                table: "specials",
                columns: new[] { "id", "created_at", "created_by_user_id", "cron_schedule", "deactivated_at", "deactivated_by_user_id", "description", "end_date", "end_time", "is_active", "is_recurring", "special_category_id", "start_date", "start_time", "title", "updated_at", "updated_by_user_id", "venue_id" },
                values: new object[,]
                {
                    { 1L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", "0 21 * * 5,6", null, null, "Live music showcasing the best in local, regional, and national talent. Various genres from rock to jazz.", null, new NodaTime.LocalTime(23, 0), true, true, 3, new NodaTime.LocalDate(2025, 5, 3), new NodaTime.LocalTime(21, 0), "Live Music Friday/Saturday", null, null, 1L },
                    { 2L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", "0 16 * * 1-5", null, null, "Enjoy $1 off all draft beers and $5 house wines.", null, new NodaTime.LocalTime(18, 0), true, true, 2, new NodaTime.LocalDate(2025, 5, 1), new NodaTime.LocalTime(16, 0), "Happy Hour", null, null, 1L },
                    { 3L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", null, null, null, "Sweet and spicy chicken sandwich with sweet n spicy sauce, lettuce, and pickles.", new NodaTime.LocalDate(2025, 5, 27), new NodaTime.LocalTime(22, 0), true, false, 1, new NodaTime.LocalDate(2025, 5, 20), new NodaTime.LocalTime(11, 0), "Weekly Burger Special - Sweet & Spicy Chicken Sandwich", null, null, 2L },
                    { 4L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", "0 21 * * 3", null, null, "Pub Trivia with first and second place prizes. Sponsored by Bacardi Oakheart.", null, new NodaTime.LocalTime(23, 0), true, true, 3, new NodaTime.LocalDate(2025, 5, 22), new NodaTime.LocalTime(21, 0), "Wednesday Night Quizzo", null, null, 2L },
                    { 5L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", "0 11 * * 2", null, null, "Every Tuesday is Mug Club Night. Our valued Mug club members enjoy their First beer, of their choice, on US!!", null, new NodaTime.LocalTime(23, 0), true, true, 2, new NodaTime.LocalDate(2025, 5, 21), new NodaTime.LocalTime(11, 0), "Mug Club Tuesday", null, null, 2L },
                    { 6L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", "0 10 * * 0", null, null, "Special brunch menu served from 10am to 2pm every Sunday.", null, new NodaTime.LocalTime(14, 0), true, true, 1, new NodaTime.LocalDate(2025, 5, 18), new NodaTime.LocalTime(10, 0), "Sunday Brunch", null, null, 3L },
                    { 7L, NodaTime.Instant.FromUnixTimeTicks(0L), "system-seed", "0 16 * * 2-6", null, null, "Enjoy our specially crafted cocktails at a reduced price.", null, new NodaTime.LocalTime(18, 0), true, true, 2, new NodaTime.LocalDate(2025, 5, 21), new NodaTime.LocalTime(16, 0), "Cocktail Hour", null, null, 3L }
                });

            migrationBuilder.CreateIndex(
                name: "ix_activity_threads_expires_at",
                table: "activity_threads",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "ix_activity_threads_venue_id",
                table: "activity_threads",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "ix_activity_threads_venue_id_is_active",
                table: "activity_threads",
                columns: new[] { "venue_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_business_hours_day_of_week_id",
                table: "business_hours",
                column: "day_of_week_id");

            migrationBuilder.CreateIndex(
                name: "ix_business_hours_venue_id_day_of_week_id",
                table: "business_hours",
                columns: new[] { "venue_id", "day_of_week_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_days_of_week_iso_number",
                table: "days_of_week",
                column: "iso_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_media_created_at",
                table: "media",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_media_media_type",
                table: "media",
                column: "media_type");

            migrationBuilder.CreateIndex(
                name: "ix_media_post_id",
                table: "media",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "ix_media_venue_id",
                table: "media",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_created_at",
                table: "notifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_recipient_user_id",
                table: "notifications",
                column: "recipient_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_recipient_user_id_is_read",
                table: "notifications",
                columns: new[] { "recipient_user_id", "is_read" });

            migrationBuilder.CreateIndex(
                name: "ix_notifications_related_special_id",
                table: "notifications",
                column: "related_special_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_related_tag_id",
                table: "notifications",
                column: "related_tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_related_venue_id",
                table: "notifications",
                column: "related_venue_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_sender_user_id",
                table: "notifications",
                column: "sender_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_type",
                table: "notifications",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "ix_permissions_category_action_resource",
                table: "permissions",
                columns: new[] { "category", "action", "resource" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_permissions_is_active",
                table: "permissions",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_permissions_name",
                table: "permissions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_post_vibes_vibe_id",
                table: "post_vibes",
                column: "vibe_id");

            migrationBuilder.CreateIndex(
                name: "ix_posts_activity_thread_id",
                table: "posts",
                column: "activity_thread_id");

            migrationBuilder.CreateIndex(
                name: "ix_posts_activity_thread_id_is_active",
                table: "posts",
                columns: new[] { "activity_thread_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_posts_created_at",
                table: "posts",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_posts_deleted_by_user_id",
                table: "posts",
                column: "deleted_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_posts_user_id",
                table: "posts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_is_active",
                table: "roles",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_roles_name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_special_categories_name",
                table: "special_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_special_tags_tag_id",
                table: "special_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_specials_special_category_id",
                table: "specials",
                column: "special_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_specials_venue_id",
                table: "specials",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "ix_tags_is_active",
                table: "tags",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_tags_name",
                table: "tags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tags_usage_count",
                table: "tags",
                column: "usage_count");

            migrationBuilder.CreateIndex(
                name: "ix_user_follows_tag_id_is_active",
                table: "user_follows",
                columns: new[] { "tag_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_user_follows_user_id_follow_type",
                table: "user_follows",
                columns: new[] { "user_id", "follow_type" });

            migrationBuilder.CreateIndex(
                name: "ix_user_follows_user_id_tag_id_follow_type",
                table: "user_follows",
                columns: new[] { "user_id", "tag_id", "follow_type" },
                unique: true,
                filter: "is_active = true AND tag_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_user_follows_user_id_venue_id_follow_type",
                table: "user_follows",
                columns: new[] { "user_id", "venue_id", "follow_type" },
                unique: true,
                filter: "is_active = true AND venue_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_user_follows_venue_id_is_active",
                table: "user_follows",
                columns: new[] { "venue_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_assigned_by_user_id",
                table: "user_roles",
                column: "assigned_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_deactivated_by_user_id",
                table: "user_roles",
                column: "deactivated_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_user_id",
                table: "user_roles",
                column: "user_id",
                filter: "is_active = true");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_user_id_role_id",
                table: "user_roles",
                columns: new[] { "user_id", "role_id" },
                unique: true,
                filter: "is_active = true");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_provider_id",
                table: "users",
                column: "provider_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_venue_categories_name",
                table: "venue_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_venue_roles_assigned_by_user_id",
                table: "venue_roles",
                column: "assigned_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_venue_roles_deactivated_by_user_id",
                table: "venue_roles",
                column: "deactivated_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_venue_roles_role_id",
                table: "venue_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_venue_roles_user_id_venue_id_role_id",
                table: "venue_roles",
                columns: new[] { "user_id", "venue_id", "role_id" },
                unique: true,
                filter: "is_active = true");

            migrationBuilder.CreateIndex(
                name: "ix_venue_roles_venue_id_role_id",
                table: "venue_roles",
                columns: new[] { "venue_id", "role_id" },
                filter: "is_active = true");

            migrationBuilder.CreateIndex(
                name: "ix_venues_category_id",
                table: "venues",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_venues_location",
                table: "venues",
                column: "location")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "ix_venues_name",
                table: "venues",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_vibes_is_active",
                table: "vibes",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_vibes_name",
                table: "vibes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vibes_usage_count",
                table: "vibes",
                column: "usage_count");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "business_hours");

            migrationBuilder.DropTable(
                name: "media");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "post_vibes");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "special_tags");

            migrationBuilder.DropTable(
                name: "user_follows");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "venue_roles");

            migrationBuilder.DropTable(
                name: "days_of_week");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "vibes");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "specials");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "activity_threads");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "special_categories");

            migrationBuilder.DropTable(
                name: "venues");

            migrationBuilder.DropTable(
                name: "venue_categories");
        }
    }
}
