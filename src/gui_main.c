/**
 * @file gui_main.c
 * @brief GTK3 Graphical User Interface for Game Config Switcher (GCS).
 * Handles all CRUD operations for games, profiles, and file pairs directly in memory,
 * and interfaces with the core GCS logic for file operations and variable substitution.
 */

#include <gtk/gtk.h>
#include "include/gcs.h"

/* --- Global UI State --- */
Profile *g_profiles = NULL;

GtkWidget *window;
GtkWidget *combo_game;
GtkWidget *combo_profile;
GtkWidget *tree_files;
GtkListStore *store_files;
GtkWidget *entry_vars;
GtkWidget *btn_apply; // Exposed globally to allow keyboard shortcuts to trigger it

enum { COL_SRC = 0, COL_DST, NUM_COLS };

/* --- Forward Declarations --- */
static void refresh_game_combo();
static void refresh_profile_combo();
static void refresh_files_tree();
static void on_game_changed(GtkComboBox *combo, gpointer data);
static void on_profile_changed(GtkComboBox *combo, gpointer data);

/* --- Helpers --- */

/**
 * @brief Displays a modal message dialog to the user.
 * @param type The type of GTK message (INFO, WARNING, ERROR, etc.).
 * @param msg The string message to display.
 */
static void show_msg(GtkMessageType type, const gchar *msg) {
    GtkWidget *dialog = gtk_message_dialog_new(GTK_WINDOW(window), GTK_DIALOG_MODAL, type, GTK_BUTTONS_OK, "%s", msg);
    gtk_dialog_run(GTK_DIALOG(dialog));
    gtk_widget_destroy(dialog);
}

/**
 * @brief Prompts the user with a Yes/No question dialog.
 * @param msg The question to ask.
 * @return TRUE if the user clicked Yes, FALSE otherwise.
 */
static gboolean ask_confirm(const gchar *msg) {
    GtkWidget *dialog = gtk_message_dialog_new(GTK_WINDOW(window), GTK_DIALOG_MODAL, GTK_MESSAGE_QUESTION, GTK_BUTTONS_YES_NO, "%s", msg);
    gint response = gtk_dialog_run(GTK_DIALOG(dialog));
    gtk_widget_destroy(dialog);
    return (response == GTK_RESPONSE_YES);
}

/**
 * @brief Retrieves the Profile struct corresponding to the currently selected dropdowns.
 * @return A pointer to the Profile, or NULL if none are selected/found.
 */
static Profile* get_current_profile() {
    gchar *game = gtk_combo_box_text_get_active_text(GTK_COMBO_BOX_TEXT(combo_game));
    gchar *prof = gtk_combo_box_text_get_active_text(GTK_COMBO_BOX_TEXT(combo_profile));
    
    if (!game || !prof) {
        g_free(game); g_free(prof);
        return NULL;
    }

    Profile *curr = g_profiles;
    Profile *found = NULL;
    while (curr) {
        if (strcmp(curr->game_name, game) == 0 && strcmp(curr->profile_name, prof) == 0) {
            found = curr;
            break;
        }
        curr = curr->next;
    }
    
    g_free(game);
    g_free(prof);
    return found;
}

/* --- Dialogs & Actions: Games & Profiles --- */

/**
 * @brief Callback to add a new Game (requires an initial profile).
 */
static void on_add_game_clicked(GtkWidget *widget, gpointer data) {
    (void)widget; (void)data;
    GtkWidget *dialog = gtk_dialog_new_with_buttons("Add New Game", GTK_WINDOW(window), GTK_DIALOG_MODAL,
                                                    "_Cancel", GTK_RESPONSE_CANCEL, "_Add", GTK_RESPONSE_ACCEPT, NULL);

    GtkWidget *content = gtk_dialog_get_content_area(GTK_DIALOG(dialog));
    GtkWidget *grid = gtk_grid_new();
    gtk_grid_set_row_spacing(GTK_GRID(grid), 5);
    gtk_grid_set_column_spacing(GTK_GRID(grid), 10);
    gtk_container_set_border_width(GTK_CONTAINER(grid), 10);
    
    GtkWidget *entry_g = gtk_entry_new();
    GtkWidget *entry_p = gtk_entry_new();
    gtk_entry_set_text(GTK_ENTRY(entry_p), "Default");

    gtk_grid_attach(GTK_GRID(grid), gtk_label_new("New Game Name:"), 0, 0, 1, 1);
    gtk_grid_attach(GTK_GRID(grid), entry_g, 1, 0, 1, 1);
    gtk_grid_attach(GTK_GRID(grid), gtk_label_new("Initial Profile:"), 0, 1, 1, 1);
    gtk_grid_attach(GTK_GRID(grid), entry_p, 1, 1, 1, 1);

    gtk_container_add(GTK_CONTAINER(content), grid);
    gtk_widget_show_all(dialog);

    if (gtk_dialog_run(GTK_DIALOG(dialog)) == GTK_RESPONSE_ACCEPT) {
        const char *g_name = gtk_entry_get_text(GTK_ENTRY(entry_g));
        const char *p_name = gtk_entry_get_text(GTK_ENTRY(entry_p));

        if (strlen(g_name) > 0 && strlen(p_name) > 0) {
            Profile *new_prof = malloc(sizeof(Profile));
            new_prof->game_name = strdup(g_name);
            new_prof->profile_name = strdup(p_name);
            new_prof->files = NULL;
            new_prof->next = g_profiles;
            g_profiles = new_prof;
            
            save_config(g_profiles);
            refresh_game_combo();
        }
    }
    gtk_widget_destroy(dialog);
}

/**
 * @brief Callback to edit the selected Game name across all its profiles.
 */
static void on_edit_game_clicked(GtkWidget *w, gpointer d) {
    (void)w; (void)d;
    gchar *game = gtk_combo_box_text_get_active_text(GTK_COMBO_BOX_TEXT(combo_game));
    if (!game) return;

    GtkWidget *dialog = gtk_dialog_new_with_buttons("Edit Game", GTK_WINDOW(window), GTK_DIALOG_MODAL,
                                                    "_Cancel", GTK_RESPONSE_CANCEL, "_Save", GTK_RESPONSE_ACCEPT, NULL);
    GtkWidget *content = gtk_dialog_get_content_area(GTK_DIALOG(dialog));
    GtkWidget *entry_g = gtk_entry_new();
    gtk_entry_set_text(GTK_ENTRY(entry_g), game);
    
    GtkWidget *box = gtk_box_new(GTK_ORIENTATION_HORIZONTAL, 10);
    gtk_container_set_border_width(GTK_CONTAINER(box), 10);
    gtk_box_pack_start(GTK_BOX(box), gtk_label_new("Game Name:"), FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(box), entry_g, TRUE, TRUE, 0);
    gtk_container_add(GTK_CONTAINER(content), box);
    gtk_widget_show_all(dialog);

    if (gtk_dialog_run(GTK_DIALOG(dialog)) == GTK_RESPONSE_ACCEPT) {
        const char *new_name = gtk_entry_get_text(GTK_ENTRY(entry_g));
        if (strlen(new_name) > 0 && strcmp(new_name, game) != 0) {
            Profile *curr = g_profiles;
            while(curr) {
                if (strcmp(curr->game_name, game) == 0) {
                    free(curr->game_name);
                    curr->game_name = strdup(new_name);
                }
                curr = curr->next;
            }
            save_config(g_profiles);
            refresh_game_combo();
        }
    }
    g_free(game);
    gtk_widget_destroy(dialog);
}

/**
 * @brief Callback to delete the selected Game and ALL its associated profiles.
 */
static void on_del_game_clicked(GtkWidget *w, gpointer d) {
    (void)w; (void)d;
    gchar *game = gtk_combo_box_text_get_active_text(GTK_COMBO_BOX_TEXT(combo_game));
    if (!game) return;

    gchar *msg = g_strdup_printf("Are you sure you want to delete '%s' and ALL its profiles?", game);
    if (ask_confirm(msg)) {
        Profile *curr = g_profiles, *prev = NULL;
        while (curr) {
            if (strcmp(curr->game_name, game) == 0) {
                Profile *to_del = curr;
                if (prev) prev->next = curr->next;
                else g_profiles = curr->next;
                curr = curr->next;
                
                free(to_del->game_name);
                free(to_del->profile_name);
                FilePair *fp = to_del->files, *tmp;
                while (fp) { tmp = fp; fp = fp->next; free(tmp->src); free(tmp->dst); free(tmp); }
                free(to_del);
            } else {
                prev = curr;
                curr = curr->next;
            }
        }
        save_config(g_profiles);
        refresh_game_combo();
    }
    g_free(msg); g_free(game);
}

/**
 * @brief Callback to add a new profile to the CURRENTLY selected game.
 */
static void on_add_profile_clicked(GtkWidget *widget, gpointer data) {
    (void)widget; (void)data;
    gchar *game = gtk_combo_box_text_get_active_text(GTK_COMBO_BOX_TEXT(combo_game));
    if (!game) {
        show_msg(GTK_MESSAGE_WARNING, "Please select or add a game first.");
        return;
    }

    GtkWidget *dialog = gtk_dialog_new_with_buttons("Add Profile to Game", GTK_WINDOW(window), GTK_DIALOG_MODAL,
                                                    "_Cancel", GTK_RESPONSE_CANCEL, "_Add", GTK_RESPONSE_ACCEPT, NULL);

    GtkWidget *content = gtk_dialog_get_content_area(GTK_DIALOG(dialog));
    GtkWidget *grid = gtk_grid_new();
    gtk_grid_set_row_spacing(GTK_GRID(grid), 5);
    gtk_grid_set_column_spacing(GTK_GRID(grid), 10);
    gtk_container_set_border_width(GTK_CONTAINER(grid), 10);
    
    GtkWidget *entry_p = gtk_entry_new();
    
    gchar *label_text = g_strdup_printf("Adding to: %s", game);
    gtk_grid_attach(GTK_GRID(grid), gtk_label_new(label_text), 0, 0, 2, 1);
    gtk_grid_attach(GTK_GRID(grid), gtk_label_new("Profile Name:"), 0, 1, 1, 1);
    gtk_grid_attach(GTK_GRID(grid), entry_p, 1, 1, 1, 1);

    gtk_container_add(GTK_CONTAINER(content), grid);
    gtk_widget_show_all(dialog);

    if (gtk_dialog_run(GTK_DIALOG(dialog)) == GTK_RESPONSE_ACCEPT) {
        const char *p_name = gtk_entry_get_text(GTK_ENTRY(entry_p));

        if (strlen(p_name) > 0) {
            Profile *new_prof = malloc(sizeof(Profile));
            new_prof->game_name = strdup(game);
            new_prof->profile_name = strdup(p_name);
            new_prof->files = NULL;
            new_prof->next = g_profiles;
            g_profiles = new_prof;
            
            save_config(g_profiles);
            refresh_profile_combo();
        }
    }
    
    g_free(label_text);
    g_free(game);
    gtk_widget_destroy(dialog);
}

/**
 * @brief Callback to edit the selected Profile name.
 */
static void on_edit_profile_clicked(GtkWidget *w, gpointer d) {
    (void)w; (void)d;
    Profile *p = get_current_profile();
    if (!p) return;

    GtkWidget *dialog = gtk_dialog_new_with_buttons("Edit Profile", GTK_WINDOW(window), GTK_DIALOG_MODAL,
                                                    "_Cancel", GTK_RESPONSE_CANCEL, "_Save", GTK_RESPONSE_ACCEPT, NULL);
    GtkWidget *content = gtk_dialog_get_content_area(GTK_DIALOG(dialog));
    GtkWidget *entry_p = gtk_entry_new();
    gtk_entry_set_text(GTK_ENTRY(entry_p), p->profile_name);
    
    GtkWidget *box = gtk_box_new(GTK_ORIENTATION_HORIZONTAL, 10);
    gtk_container_set_border_width(GTK_CONTAINER(box), 10);
    gtk_box_pack_start(GTK_BOX(box), gtk_label_new("Profile Name:"), FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(box), entry_p, TRUE, TRUE, 0);
    gtk_container_add(GTK_CONTAINER(content), box);
    gtk_widget_show_all(dialog);

    if (gtk_dialog_run(GTK_DIALOG(dialog)) == GTK_RESPONSE_ACCEPT) {
        const char *new_name = gtk_entry_get_text(GTK_ENTRY(entry_p));
        if (strlen(new_name) > 0) {
            free(p->profile_name);
            p->profile_name = strdup(new_name);
            save_config(g_profiles);
            refresh_profile_combo();
        }
    }
    gtk_widget_destroy(dialog);
}

/**
 * @brief Callback to delete the selected Profile.
 */
static void on_del_profile_clicked(GtkWidget *w, gpointer d) {
    (void)w; (void)d;
    Profile *p = get_current_profile();
    if (!p) return;

    gchar *msg = g_strdup_printf("Delete profile '%s'?", p->profile_name);
    if (ask_confirm(msg)) {
        Profile *curr = g_profiles, *prev = NULL;
        while (curr) {
            if (curr == p) {
                if (prev) prev->next = curr->next;
                else g_profiles = curr->next;
                
                free(curr->game_name);
                free(curr->profile_name);
                FilePair *fp = curr->files, *tmp;
                while (fp) { tmp = fp; fp = fp->next; free(tmp->src); free(tmp->dst); free(tmp); }
                free(curr);
                break;
            }
            prev = curr;
            curr = curr->next;
        }
        save_config(g_profiles);
        refresh_game_combo();
    }
    g_free(msg);
}

/* --- Dialogs & Actions: File Pairs --- */

/**
 * @brief Reusable callback to open a standard file chooser dialog and populate an entry.
 */
static void on_browse_clicked(GtkWidget *btn, gpointer entry_ptr) {
    GtkWidget *entry = GTK_WIDGET(entry_ptr);
    gboolean is_dst = GPOINTER_TO_INT(g_object_get_data(G_OBJECT(btn), "is_dst"));
    
    GtkWidget *dialog = gtk_file_chooser_dialog_new(
        is_dst ? "Select Destination File" : "Select Source File",
        GTK_WINDOW(window),
        GTK_FILE_CHOOSER_ACTION_OPEN,
        "_Cancel", GTK_RESPONSE_CANCEL,
        "_Select", GTK_RESPONSE_ACCEPT,
        NULL);

    if (gtk_dialog_run(GTK_DIALOG(dialog)) == GTK_RESPONSE_ACCEPT) {
        char *filename = gtk_file_chooser_get_filename(GTK_FILE_CHOOSER(dialog));
        gtk_entry_set_text(GTK_ENTRY(entry), filename);
        g_free(filename);
    }
    gtk_widget_destroy(dialog);
}

/**
 * @brief Core function for Add/Edit File Pair dialog to avoid UI duplication.
 * @param existing_pair Pass NULL to add new, or an existing FilePair to edit.
 * @param target The Profile to attach the pair to.
 */
static void open_file_pair_dialog(FilePair *existing_pair, Profile *target) {
    gboolean is_edit = (existing_pair != NULL);
    GtkWidget *dialog = gtk_dialog_new_with_buttons(is_edit ? "Edit File Pair" : "Add File Pair", 
                                                    GTK_WINDOW(window), GTK_DIALOG_MODAL,
                                                    "_Cancel", GTK_RESPONSE_CANCEL, 
                                                    is_edit ? "_Save" : "_Add", GTK_RESPONSE_ACCEPT, NULL);
    GtkWidget *content = gtk_dialog_get_content_area(GTK_DIALOG(dialog));
    GtkWidget *grid = gtk_grid_new();
    gtk_grid_set_row_spacing(GTK_GRID(grid), 5);
    gtk_grid_set_column_spacing(GTK_GRID(grid), 5);
    gtk_container_set_border_width(GTK_CONTAINER(grid), 10);

    GtkWidget *entry_src = gtk_entry_new();
    GtkWidget *entry_dst = gtk_entry_new();
    gtk_widget_set_hexpand(entry_src, TRUE);
    gtk_widget_set_hexpand(entry_dst, TRUE);

    if (is_edit) {
        gtk_entry_set_text(GTK_ENTRY(entry_src), existing_pair->src);
        gtk_entry_set_text(GTK_ENTRY(entry_dst), existing_pair->dst);
    }

    GtkWidget *btn_browse_src = gtk_button_new_with_label("...");
    GtkWidget *btn_browse_dst = gtk_button_new_with_label("...");
    
    g_object_set_data(G_OBJECT(btn_browse_src), "is_dst", GINT_TO_POINTER(0));
    g_object_set_data(G_OBJECT(btn_browse_dst), "is_dst", GINT_TO_POINTER(1));
    
    g_signal_connect(btn_browse_src, "clicked", G_CALLBACK(on_browse_clicked), entry_src);
    g_signal_connect(btn_browse_dst, "clicked", G_CALLBACK(on_browse_clicked), entry_dst);

    gtk_grid_attach(GTK_GRID(grid), gtk_label_new("Source:"), 0, 0, 1, 1);
    gtk_grid_attach(GTK_GRID(grid), entry_src, 1, 0, 1, 1);
    gtk_grid_attach(GTK_GRID(grid), btn_browse_src, 2, 0, 1, 1);
    
    gtk_grid_attach(GTK_GRID(grid), gtk_label_new("Destination:"), 0, 1, 1, 1);
    gtk_grid_attach(GTK_GRID(grid), entry_dst, 1, 1, 1, 1);
    gtk_grid_attach(GTK_GRID(grid), btn_browse_dst, 2, 1, 1, 1);

    gtk_container_add(GTK_CONTAINER(content), grid);
    gtk_widget_show_all(dialog);

    if (gtk_dialog_run(GTK_DIALOG(dialog)) == GTK_RESPONSE_ACCEPT) {
        const gchar *src_path = gtk_entry_get_text(GTK_ENTRY(entry_src));
        const gchar *dst_path = gtk_entry_get_text(GTK_ENTRY(entry_dst));

        if (strlen(src_path) > 0 && strlen(dst_path) > 0) {
            if (is_edit) {
                free(existing_pair->src); free(existing_pair->dst);
                existing_pair->src = strdup(src_path);
                existing_pair->dst = strdup(dst_path);
            } else {
                FilePair *pair = malloc(sizeof(FilePair));
                pair->src = strdup(src_path);
                pair->dst = strdup(dst_path);
                pair->next = NULL;
                
                if (target->files == NULL) {
                    target->files = pair;
                } else {
                    FilePair *fp = target->files;
                    while (fp->next) fp = fp->next;
                    fp->next = pair;
                }
            }
            save_config(g_profiles);
            refresh_files_tree();
        }
    }
    gtk_widget_destroy(dialog);
}

/**
 * @brief Callback to add a FilePair to the currently selected profile.
 */
static void on_add_file_clicked(GtkWidget *w, gpointer d) {
    (void)w; (void)d;
    Profile *target = get_current_profile();
    if (!target) {
        show_msg(GTK_MESSAGE_WARNING, "Select a Game and Profile first.");
        return;
    }
    open_file_pair_dialog(NULL, target);
}

/**
 * @brief Callback to edit the selected FilePair in the TreeView.
 */
static void on_edit_file_clicked(GtkWidget *w, gpointer d) {
    (void)w; (void)d;
    Profile *p = get_current_profile();
    if (!p) return;

    GtkTreeSelection *sel = gtk_tree_view_get_selection(GTK_TREE_VIEW(tree_files));
    GtkTreeIter iter;
    GtkTreeModel *model;

    if (gtk_tree_selection_get_selected(sel, &model, &iter)) {
        gchar *old_src, *old_dst;
        gtk_tree_model_get(model, &iter, COL_SRC, &old_src, COL_DST, &old_dst, -1);
        
        FilePair *curr = p->files;
        while (curr) {
            if (strcmp(curr->src, old_src) == 0 && strcmp(curr->dst, old_dst) == 0) {
                open_file_pair_dialog(curr, p);
                break;
            }
            curr = curr->next;
        }
        g_free(old_src); g_free(old_dst);
    } else {
        show_msg(GTK_MESSAGE_WARNING, "Please select a file pair to edit.");
    }
}

/**
 * @brief Callback to delete the selected FilePair in the TreeView.
 */
static void on_del_file_clicked(GtkWidget *w, gpointer d) {
    (void)w; (void)d;
    Profile *p = get_current_profile();
    if (!p) return;

    GtkTreeSelection *sel = gtk_tree_view_get_selection(GTK_TREE_VIEW(tree_files));
    GtkTreeIter iter;
    GtkTreeModel *model;

    if (gtk_tree_selection_get_selected(sel, &model, &iter)) {
        gchar *old_src, *old_dst;
        gtk_tree_model_get(model, &iter, COL_SRC, &old_src, COL_DST, &old_dst, -1);
        
        if (ask_confirm("Delete this file pair mapping?")) {
            FilePair *curr = p->files, *prev = NULL;
            while (curr) {
                if (strcmp(curr->src, old_src) == 0 && strcmp(curr->dst, old_dst) == 0) {
                    if (prev) prev->next = curr->next;
                    else p->files = curr->next;
                    
                    free(curr->src); free(curr->dst); free(curr);
                    break;
                }
                prev = curr;
                curr = curr->next;
            }
            save_config(g_profiles);
            refresh_files_tree();
        }
        g_free(old_src); g_free(old_dst);
    } else {
        show_msg(GTK_MESSAGE_WARNING, "Please select a file pair to delete.");
    }
}

/* --- Execution --- */

/**
 * @brief Core execution logic managing tokenization of variables and calling GCS ops.
 * @param use_all If TRUE, executes all profiles matching the name globally.
 */
static void execute_with_vars(gboolean use_all) {
    const gchar *var_text = gtk_entry_get_text(GTK_ENTRY(entry_vars));
    gchar **tokens = g_strsplit(var_text, " ", 20);
    
    int var_count = 0;
    char *vars[20] = {0};
    while (tokens[var_count] != NULL && var_count < 20) {
        if (strlen(tokens[var_count]) > 0) {
            vars[var_count] = tokens[var_count];
        }
        var_count++;
    }

    if (use_all) {
        gchar *prof = gtk_combo_box_text_get_active_text(GTK_COMBO_BOX_TEXT(combo_profile));
        if (!prof) {
            show_msg(GTK_MESSAGE_WARNING, "Select a profile name to use globally.");
        } else {
            execute_all_named_profiles(prof, var_count, vars);
            gchar *msg = g_strdup_printf("Applied profile '%s' across all matching games.", prof);
            show_msg(GTK_MESSAGE_INFO, msg);
            g_free(msg);
            g_free(prof);
        }
    } else {
        Profile *p = get_current_profile();
        if (p) {
            execute_profile(p, var_count, vars);
            show_msg(GTK_MESSAGE_INFO, "Profile applied successfully.");
        } else {
            show_msg(GTK_MESSAGE_WARNING, "Select a valid profile first.");
        }
    }
    g_strfreev(tokens);
}

static void on_apply_clicked(GtkWidget *w, gpointer d) { (void)w; (void)d; execute_with_vars(FALSE); }
static void on_useall_clicked(GtkWidget *w, gpointer d) { (void)w; (void)d; execute_with_vars(TRUE); }

/**
 * @brief Captures 'Enter' key press on the variables text box to trigger Apply.
 */
static void on_entry_activate(GtkEntry *entry, gpointer user_data) {
    (void)entry; (void)user_data;
    gtk_button_clicked(GTK_BUTTON(btn_apply));
}

/* --- UI Refresh Logic --- */

/**
 * @brief Reloads the TreeView to reflect the current profile's file pairs.
 */
static void refresh_files_tree() {
    gtk_list_store_clear(store_files);
    Profile *p = get_current_profile();
    if (!p) return;

    FilePair *curr = p->files;
    GtkTreeIter iter;
    while (curr) {
        gtk_list_store_append(store_files, &iter);
        gtk_list_store_set(store_files, &iter, COL_SRC, curr->src, COL_DST, curr->dst, -1);
        curr = curr->next;
    }
}

/**
 * @brief Reloads the Profile combobox based on the active Game selection.
 */
static void refresh_profile_combo() {
    g_signal_handlers_block_by_func(combo_profile, on_profile_changed, NULL);
    gtk_combo_box_text_remove_all(GTK_COMBO_BOX_TEXT(combo_profile));
    
    gchar *selected_game = gtk_combo_box_text_get_active_text(GTK_COMBO_BOX_TEXT(combo_game));
    if (!selected_game) {
        g_signal_handlers_unblock_by_func(combo_profile, on_profile_changed, NULL);
        refresh_files_tree();
        return;
    }

    Profile *curr = g_profiles;
    int count = 0;
    while (curr) {
        if (strcmp(curr->game_name, selected_game) == 0) {
            gtk_combo_box_text_append_text(GTK_COMBO_BOX_TEXT(combo_profile), curr->profile_name);
            count++;
        }
        curr = curr->next;
    }
    
    g_free(selected_game);

    if (count > 0) gtk_combo_box_set_active(GTK_COMBO_BOX(combo_profile), 0);
    g_signal_handlers_unblock_by_func(combo_profile, on_profile_changed, NULL);
    refresh_files_tree();
}

/**
 * @brief Scans memory for unique games and populates the Game combobox.
 */
static void refresh_game_combo() {
    g_signal_handlers_block_by_func(combo_game, on_game_changed, NULL);
    gtk_combo_box_text_remove_all(GTK_COMBO_BOX_TEXT(combo_game));

    Profile *curr = g_profiles;
    GList *games = NULL;

    while (curr) {
        if (!g_list_find_custom(games, curr->game_name, (GCompareFunc)strcmp)) {
            games = g_list_append(games, curr->game_name);
            gtk_combo_box_text_append_text(GTK_COMBO_BOX_TEXT(combo_game), curr->game_name);
        }
        curr = curr->next;
    }
    
    if (games) {
        gtk_combo_box_set_active(GTK_COMBO_BOX(combo_game), 0);
        g_list_free(games);
    }
    
    g_signal_handlers_unblock_by_func(combo_game, on_game_changed, NULL);
    refresh_profile_combo();
}

static void on_game_changed(GtkComboBox *c, gpointer d) { (void)c; (void)d; refresh_profile_combo(); }
static void on_profile_changed(GtkComboBox *c, gpointer d) { (void)c; (void)d; refresh_files_tree(); }

/**
 * @brief Cleans up memory and exits GTK main loop.
 */
static void on_window_closed(GtkWidget *w, gpointer d) {
    (void)w; (void)d;
    if (g_profiles) free_profiles(g_profiles);
    gtk_main_quit();
}

/* --- Main Application --- */

int main(int argc, char *argv[]) {
    gtk_init(&argc, &argv);
    g_profiles = load_config();

    window = gtk_window_new(GTK_WINDOW_TOPLEVEL);
    gtk_window_set_title(GTK_WINDOW(window), "GCS Config Manager");
    gtk_window_set_default_size(GTK_WINDOW(window), 700, 500);
    gtk_container_set_border_width(GTK_CONTAINER(window), 10);
    g_signal_connect(window, "destroy", G_CALLBACK(on_window_closed), NULL);

    GtkWidget *vbox_main = gtk_box_new(GTK_ORIENTATION_VERTICAL, 10);
    gtk_container_add(GTK_CONTAINER(window), vbox_main);

    /* --- Top: Selection & Creation --- */
    GtkWidget *hbox_top = gtk_box_new(GTK_ORIENTATION_HORIZONTAL, 15);
    gtk_box_pack_start(GTK_BOX(vbox_main), hbox_top, FALSE, FALSE, 0);

    // Game Layout
    GtkWidget *vbox_game = gtk_box_new(GTK_ORIENTATION_VERTICAL, 2);
    GtkWidget *hbox_g_ctrl = gtk_box_new(GTK_ORIENTATION_HORIZONTAL, 2);
    combo_game = gtk_combo_box_text_new();
    GtkWidget *btn_add_g = gtk_button_new_with_label("Add");
    GtkWidget *btn_edit_g = gtk_button_new_with_label("Edit");
    GtkWidget *btn_del_g = gtk_button_new_with_label("Delete");
    
    gtk_box_pack_start(GTK_BOX(hbox_g_ctrl), combo_game, TRUE, TRUE, 0);
    gtk_box_pack_start(GTK_BOX(hbox_g_ctrl), btn_add_g, FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(hbox_g_ctrl), btn_edit_g, FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(hbox_g_ctrl), btn_del_g, FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(vbox_game), gtk_label_new("Game:"), FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(vbox_game), hbox_g_ctrl, FALSE, FALSE, 0);
    
    // Profile Layout
    GtkWidget *vbox_prof = gtk_box_new(GTK_ORIENTATION_VERTICAL, 2);
    GtkWidget *hbox_p_ctrl = gtk_box_new(GTK_ORIENTATION_HORIZONTAL, 2);
    combo_profile = gtk_combo_box_text_new();
    GtkWidget *btn_add_p = gtk_button_new_with_label("Add");
    GtkWidget *btn_edit_p = gtk_button_new_with_label("Edit");
    GtkWidget *btn_del_p = gtk_button_new_with_label("Delete");
    
    gtk_box_pack_start(GTK_BOX(hbox_p_ctrl), combo_profile, TRUE, TRUE, 0);
    gtk_box_pack_start(GTK_BOX(hbox_p_ctrl), btn_add_p, FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(hbox_p_ctrl), btn_edit_p, FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(hbox_p_ctrl), btn_del_p, FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(vbox_prof), gtk_label_new("Profile:"), FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(vbox_prof), hbox_p_ctrl, FALSE, FALSE, 0);

    gtk_box_pack_start(GTK_BOX(hbox_top), vbox_game, TRUE, TRUE, 0);
    gtk_box_pack_start(GTK_BOX(hbox_top), vbox_prof, TRUE, TRUE, 0);

    // Wire up Top bar signals
    g_signal_connect(combo_game, "changed", G_CALLBACK(on_game_changed), NULL);
    g_signal_connect(combo_profile, "changed", G_CALLBACK(on_profile_changed), NULL);
    g_signal_connect(btn_add_g, "clicked", G_CALLBACK(on_add_game_clicked), NULL);
    g_signal_connect(btn_edit_g, "clicked", G_CALLBACK(on_edit_game_clicked), NULL);
    g_signal_connect(btn_del_g, "clicked", G_CALLBACK(on_del_game_clicked), NULL);
    g_signal_connect(btn_add_p, "clicked", G_CALLBACK(on_add_profile_clicked), NULL);
    g_signal_connect(btn_edit_p, "clicked", G_CALLBACK(on_edit_profile_clicked), NULL);
    g_signal_connect(btn_del_p, "clicked", G_CALLBACK(on_del_profile_clicked), NULL);

    /* --- Middle: Files TreeView --- */
    GtkWidget *frame_files = gtk_frame_new("Config Files attached to Profile");
    gtk_box_pack_start(GTK_BOX(vbox_main), frame_files, TRUE, TRUE, 0);
    
    GtkWidget *vbox_files = gtk_box_new(GTK_ORIENTATION_VERTICAL, 5);
    gtk_container_set_border_width(GTK_CONTAINER(vbox_files), 5);
    gtk_container_add(GTK_CONTAINER(frame_files), vbox_files);

    store_files = gtk_list_store_new(NUM_COLS, G_TYPE_STRING, G_TYPE_STRING);
    tree_files = gtk_tree_view_new_with_model(GTK_TREE_MODEL(store_files));
    gtk_tree_view_append_column(GTK_TREE_VIEW(tree_files), gtk_tree_view_column_new_with_attributes("Source Template", gtk_cell_renderer_text_new(), "text", COL_SRC, NULL));
    gtk_tree_view_append_column(GTK_TREE_VIEW(tree_files), gtk_tree_view_column_new_with_attributes("Destination", gtk_cell_renderer_text_new(), "text", COL_DST, NULL));
    
    GtkWidget *scroll = gtk_scrolled_window_new(NULL, NULL);
    gtk_container_add(GTK_CONTAINER(scroll), tree_files);
    gtk_box_pack_start(GTK_BOX(vbox_files), scroll, TRUE, TRUE, 0);

    // TreeView Action Buttons
    GtkWidget *hbox_file_btns = gtk_box_new(GTK_ORIENTATION_HORIZONTAL, 5);
    GtkWidget *btn_add_f = gtk_button_new_with_label("Add Pair");
    GtkWidget *btn_edit_f = gtk_button_new_with_label("Edit Pair");
    GtkWidget *btn_del_f = gtk_button_new_with_label("Delete Pair");
    
    gtk_box_pack_start(GTK_BOX(hbox_file_btns), btn_add_f, FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(hbox_file_btns), btn_edit_f, FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(hbox_file_btns), btn_del_f, FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(vbox_files), hbox_file_btns, FALSE, FALSE, 0);

    g_signal_connect(btn_add_f, "clicked", G_CALLBACK(on_add_file_clicked), NULL);
    g_signal_connect(btn_edit_f, "clicked", G_CALLBACK(on_edit_file_clicked), NULL);
    g_signal_connect(btn_del_f, "clicked", G_CALLBACK(on_del_file_clicked), NULL);

    /* --- Bottom: Execution & Variables --- */
    GtkWidget *frame_exec = gtk_frame_new("Execution Settings");
    gtk_box_pack_start(GTK_BOX(vbox_main), frame_exec, FALSE, FALSE, 0);
    GtkWidget *hbox_exec = gtk_box_new(GTK_ORIENTATION_HORIZONTAL, 10);
    gtk_container_set_border_width(GTK_CONTAINER(hbox_exec), 5);
    gtk_container_add(GTK_CONTAINER(frame_exec), hbox_exec);

    entry_vars = gtk_entry_new();
    gtk_entry_set_placeholder_text(GTK_ENTRY(entry_vars), "Variables e.g., RES:1080p FPS:60");
    gtk_box_pack_start(GTK_BOX(hbox_exec), entry_vars, TRUE, TRUE, 0);

    btn_apply = gtk_button_new_with_label("Apply Selected Profile");
    GtkWidget *btn_useall = gtk_button_new_with_label("Use All (Global Apply)");
    g_signal_connect(btn_apply, "clicked", G_CALLBACK(on_apply_clicked), NULL);
    g_signal_connect(btn_useall, "clicked", G_CALLBACK(on_useall_clicked), NULL);
    g_signal_connect(entry_vars, "activate", G_CALLBACK(on_entry_activate), NULL); // Allows Enter key to trigger Apply
    
    gtk_box_pack_start(GTK_BOX(hbox_exec), btn_apply, FALSE, FALSE, 0);
    gtk_box_pack_start(GTK_BOX(hbox_exec), btn_useall, FALSE, FALSE, 0);

    // Populate Initial Data
    refresh_game_combo();

    gtk_widget_show_all(window);
    gtk_main();

    return 0;
}